//===========================================================================================
// MbCompression, Complete compression library for ASP.NET 2.0-3.5 (VS2005-VS2008)
// Developed by: Miron Abramson. http://blog.mironabramson.com
// Project site: http://www.codeplex.com/MbCompression
// File last update: 15-08-09
//===========================================================================================

#region Using
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using System.Collections.Generic;
#endregion

namespace JasonSoft.Web.Compression
{
    internal class AutoModeFilterStream : Stream
    {
        private static readonly Regex REGEX_SCRIPT = new Regex("<script\\s*[^<]*src=\"((?=(?!http:|https:)[^\"]*)[^\"]*)\"\\s*[^>]*>[^<]*(?:</script>)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly Regex REGEX_STYLE = new Regex("<link\\s*[^<]*href=\"((?=(?!http:|https:)[^\"]*)[^\"]*)\"\\s*[^>]*>[^<]*(?:>)?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private const string CSS_TEMPLATE = "<link type=\"text/css\" href=\"{0}css.axd?d={1}\" rel=\"stylesheet\" />";
        private const string JS_TEMPLATE = "<script type=\"text/javascript\" src=\"{0}jslib.axd?d={1}\"></script>";

        internal AutoModeFilterStream(Stream stream, Encoding currentEncoding, bool processScripts, bool processStyles, bool combineCss,bool combineHeaderScripts)
        {
            _baseStream = stream;
            _currentEncoding = currentEncoding;
            _processScripts = processScripts;
            _processStyles = processStyles;
            _combineCss = combineCss;
            _combineHeaderScripts = combineHeaderScripts;
            _cssFiles = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            _headerScriptFiles = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
            RemoveHeaderScriptMatchEvaluator = new MatchEvaluator(RemoveHeaderScript);
            RemoveCssMatchEvaluator = new MatchEvaluator(RemoveCss);
        }

        private readonly Stream _baseStream;
        private readonly Encoding _currentEncoding;
        private readonly bool _processScripts;
        private readonly bool _processStyles;
        private readonly bool _combineCss;
        private readonly bool _combineHeaderScripts;
        private int _cssHeaderIndex = -1;
        private int _jsHeaderIndex = -1;
        private readonly Dictionary<string, List<string>> _cssFiles;
        private readonly Dictionary<string, List<string>> _headerScriptFiles;
        private static readonly MatchEvaluator ScriptFoundMatchEvaluator = new MatchEvaluator(ScriptFound);
        private readonly MatchEvaluator RemoveHeaderScriptMatchEvaluator;
        private static readonly MatchEvaluator StyleFoundMatchEvaluator = new MatchEvaluator(StyleFound);
        private readonly MatchEvaluator RemoveCssMatchEvaluator;

        #region Properites

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
            _baseStream.Flush();
        }

        public override long Length
        {
            get { return 0; }
        }

        private long _position;
        public override long Position
        {
            get { return _position; }
            set { _position = value; }
        }

        #endregion

        #region Methods

        public override int Read(byte[] buffer, int offset, int count)
        {
            return _baseStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return _baseStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            _baseStream.SetLength(value);
        }

        public override void Close()
        {
            _baseStream.Close();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (!_processStyles && !_processScripts)
            {
                _baseStream.Write(buffer, offset, count);
                return;
            }

            string html = _currentEncoding.GetString(buffer, offset, count);

            if (_processStyles)
            {
                if (_combineCss)
                {
                    html = CombineStylesUrl(html);
                }
                else
                {
                    html = FixStylesUrl(html);
                }
            }
            if (_processScripts)
            {
                if (_combineHeaderScripts)
                {
                    html = CombineHeaderScriptsUrl(html);
                }
                else
                {
                    html = FixScriptsUrl(html);
                }
            }

            byte[] finalBuffer = _currentEncoding.GetBytes(html);

            _baseStream.Write(finalBuffer, 0, finalBuffer.Length);
        }

        #region Fix style urls
        /// <summary>
        /// Fix the styles url that it can be compressed
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private static string FixStylesUrl(string html)
        {
            return REGEX_STYLE.Replace(html, StyleFoundMatchEvaluator);
        }

        /// <summary>
        /// Find the source
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private static string StyleFound(Match m)
        {
            Group g = m.Groups[1];
            if (g.Value.IndexOf(".axd", StringComparison.OrdinalIgnoreCase) > -1 ||
                m.Value.IndexOf("rel=\"stylesheet\"", StringComparison.OrdinalIgnoreCase) < 1 ||
                g.Value.Contains("?"))
            {
                return m.Value;
            }
            int index = g.Value.LastIndexOf('/');
            if (index >= 0)
            {
                index = g.Index - m.Index + index + 1;
                return m.Value.Insert(index, "css.axd?d=").Insert(g.Index - m.Index + g.Length + 10, Settings.Instance.CssVersion);
            }
            else
            {
                index = g.Index - m.Index;
                return m.Value.Insert(index, "css.axd?d=").Insert(index + g.Length + 10, Settings.Instance.CssVersion);
            }
        }

        #endregion

        #region Combine style urls
        /// <summary>
        /// Combine the styles url and change them that they can be compressed
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private string CombineStylesUrl(string html)
        {
            // If we already passed the header, so we will not combine the followed css 
            if (_cssHeaderIndex >= 0)
            {
                return REGEX_STYLE.Replace(html, StyleFoundMatchEvaluator);
            }
            else
            { 
                string fixedHtml = REGEX_STYLE.Replace(html, RemoveCssMatchEvaluator);
                _cssHeaderIndex = fixedHtml.IndexOf("</head>", StringComparison.OrdinalIgnoreCase);
                if (_cssHeaderIndex >= 0)
                {
                    foreach (KeyValuePair<string, List<string>> pair in _cssFiles)
                    {
                        string newCss = string.Format(CSS_TEMPLATE, pair.Key, string.Join(",", pair.Value.ToArray()) + Settings.Instance.CssVersion);
                        fixedHtml = fixedHtml.Insert(_cssHeaderIndex, newCss);
                    }
                }
                return fixedHtml;
            }
        }

        /// <summary>
        /// Remove the source
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private string RemoveCss(Match m)
        {
            Group g = m.Groups[1];
            if (g.Value.IndexOf(".axd", StringComparison.OrdinalIgnoreCase) > -1 ||
                m.Value.IndexOf("rel=\"stylesheet\"", StringComparison.OrdinalIgnoreCase) < 1 ||
                g.Value.Contains("?"))
            {
                return m.Value;
            }
            // We will not combine style that have specific media attribute
            if (m.Value.IndexOf("media=", StringComparison.OrdinalIgnoreCase) > -1 &&
                m.Value.IndexOf("media=\"all\"", StringComparison.OrdinalIgnoreCase) < 0)
            {
                return StyleFound(m);
            }
            int index = g.Value.LastIndexOf('/');
            string path;
            string file;
            if (index >= 0)
            {
                path = g.Value.Substring(0, index + 1);
                file = g.Value.Substring(index + 1);
            }
            else
            {
                path = string.Empty;
                file = g.Value;
            }
            List<string> l;
            if (_cssFiles.TryGetValue(path, out l))
            {
                l.Add(file);
            }
            else
            {
                l = new List<string>();
                l.Add(file);
                _cssFiles.Add(path, l);
            }
            return string.Empty;
        }

        #endregion

        #region Fix script urls

        /// <summary>
        /// Fix the scripts url that it can be compressed
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private static string FixScriptsUrl(string html)
        {
            return REGEX_SCRIPT.Replace(html, ScriptFoundMatchEvaluator);
        }

        /// <summary>
        /// Find the source
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private static string ScriptFound(Match m)
        {
            Group g = m.Groups[1];
            // Exclude .axd and .asmx handlers that serve js files and js includes that have query
            if (g.Value.Contains("?") ||
                g.Value.IndexOf(".axd", StringComparison.OrdinalIgnoreCase) > -1 ||
                g.Value.IndexOf(".asmx", StringComparison.OrdinalIgnoreCase) > -1)
            {
                return m.Value;
            }
            int index = g.Value.LastIndexOf('/');
            if (index >= 0)
            {
                index = g.Index - m.Index + index + 1;
                return m.Value.Insert(index, "jslib.axd?d=").Insert(g.Index - m.Index + g.Length + 12, Settings.Instance.JsVersion);
            }
            else
            {
                index = g.Index - m.Index;
                return m.Value.Insert(index, "jslib.axd?d=").Insert(index + g.Length + 12, Settings.Instance.JsVersion);
            }
        }
        #endregion

        #region Combine header scripts urls
        /// <summary>
        /// Combine the header scripts url and change them that they can be compressed
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        private string CombineHeaderScriptsUrl(string html)
        {
            // If we already passed the header, so we will not combine the followed scripts 
            if (_jsHeaderIndex >= 0)
            {
                return REGEX_SCRIPT.Replace(html, ScriptFoundMatchEvaluator);
            }
            else
            {
                _jsHeaderIndex = html.IndexOf("</head>", StringComparison.OrdinalIgnoreCase);
                string fixedHtml = REGEX_SCRIPT.Replace(html, RemoveHeaderScriptMatchEvaluator);
                if (_jsHeaderIndex >= 0)
                {
                    foreach (KeyValuePair<string, List<string>> pair in _headerScriptFiles)
                    {
                        string newJs = string.Format(JS_TEMPLATE, pair.Key, string.Join(",", pair.Value.ToArray()) + Settings.Instance.JsVersion);
                        fixedHtml = fixedHtml.Insert(_jsHeaderIndex, newJs);
                    }
                }
                return fixedHtml;
            }
        }

        /// <summary>
        /// Remove the source
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        private string RemoveHeaderScript(Match m)
        {
            Group g0 = m.Groups[0];
            Group g1 = m.Groups[1];
            // Exclude .axd and .asmx handlers that serve js files and js includes that have query
            if (g1.Value.Contains("?") ||
                g1.Value.IndexOf(".axd", StringComparison.OrdinalIgnoreCase) > -1 ||
                g1.Value.IndexOf(".asmx", StringComparison.OrdinalIgnoreCase) > -1)
            {
                return m.Value;
            }
            // The script is after the header tag
            if (g0.Index > _jsHeaderIndex)
            {
                return ScriptFoundMatchEvaluator(m);
            }
            int index = g1.Value.LastIndexOf('/');
            string path;
            string file;
            if (index >= 0)
            {
                path = g1.Value.Substring(0, index + 1);
                file = g1.Value.Substring(index + 1);
            }
            else
            {
                path = string.Empty;
                file = g1.Value;
            }
            List<string> l;
            if (_headerScriptFiles.TryGetValue(path, out l))
            {
                l.Add(file);
            }
            else
            {
                l = new List<string>();
                l.Add(file);
                _headerScriptFiles.Add(path, l);
            }
            _jsHeaderIndex = _jsHeaderIndex - g0.Length;
            return string.Empty;
        }

        #endregion

        #endregion
    }
}
