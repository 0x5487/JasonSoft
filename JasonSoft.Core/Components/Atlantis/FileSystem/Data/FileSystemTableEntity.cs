using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;

namespace JasonSoft.Components.Atlantis
{
    class FileSystemTableEntity
    {
    }

    public partial class FileSystem_ActiveDirectory : INotifyPropertyChanging, INotifyPropertyChanged
    {

        private static PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        private int _NodeID;

        private int _ParentNodeID;

        private string _Name;

        private string _Attribute;

        private bool _HasChild;

        private bool _IsInheritance;

        private int _Level;

        private System.Nullable<int> _RefObjectID;

        private string _RefObjectType;

        private System.DateTime _CreateDate;

        private int _CreatorID;

        private string _CreatorName;

        private System.DateTime _LastModifiedDate;

        private int _LastModifierID;

        private string _LastModifierName;

        #region Extensibility Method Definitions
        partial void OnLoaded();
        partial void OnValidate(System.Data.Linq.ChangeAction action);
        partial void OnCreated();
        partial void OnNodeIDChanging(int value);
        partial void OnNodeIDChanged();
        partial void OnParentNodeIDChanging(int value);
        partial void OnParentNodeIDChanged();
        partial void OnNameChanging(string value);
        partial void OnNameChanged();
        partial void OnAttributeChanging(string value);
        partial void OnAttributeChanged();
        partial void OnHasChildChanging(bool value);
        partial void OnHasChildChanged();
        partial void OnIsInheritanceChanging(bool value);
        partial void OnIsInheritanceChanged();
        partial void OnLevelChanging(int value);
        partial void OnLevelChanged();
        partial void OnRefObjectIDChanging(System.Nullable<int> value);
        partial void OnRefObjectIDChanged();
        partial void OnRefObjectTypeChanging(string value);
        partial void OnRefObjectTypeChanged();
        partial void OnCreateDateChanging(System.DateTime value);
        partial void OnCreateDateChanged();
        partial void OnCreatorIDChanging(int value);
        partial void OnCreatorIDChanged();
        partial void OnCreatorNameChanging(string value);
        partial void OnCreatorNameChanged();
        partial void OnLastModifiedDateChanging(System.DateTime value);
        partial void OnLastModifiedDateChanged();
        partial void OnLastModifierIDChanging(int value);
        partial void OnLastModifierIDChanged();
        partial void OnLastModifierNameChanging(string value);
        partial void OnLastModifierNameChanged();
        #endregion

        public FileSystem_ActiveDirectory()
        {
            OnCreated();
        }

        [Column(Storage = "_NodeID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int NodeID
        {
            get
            {
                return this._NodeID;
            }
            set
            {
                if ((this._NodeID != value))
                {
                    this.OnNodeIDChanging(value);
                    this.SendPropertyChanging();
                    this._NodeID = value;
                    this.SendPropertyChanged("NodeID");
                    this.OnNodeIDChanged();
                }
            }
        }

        [Column(Storage = "_ParentNodeID", DbType = "Int NOT NULL")]
        public int ParentNodeID
        {
            get
            {
                return this._ParentNodeID;
            }
            set
            {
                if ((this._ParentNodeID != value))
                {
                    this.OnParentNodeIDChanging(value);
                    this.SendPropertyChanging();
                    this._ParentNodeID = value;
                    this.SendPropertyChanged("ParentNodeID");
                    this.OnParentNodeIDChanged();
                }
            }
        }

        [Column(Storage = "_Name", DbType = "NVarChar(50) NOT NULL", CanBeNull = false)]
        public string Name
        {
            get
            {
                return this._Name;
            }
            set
            {
                if ((this._Name != value))
                {
                    this.OnNameChanging(value);
                    this.SendPropertyChanging();
                    this._Name = value;
                    this.SendPropertyChanged("Name");
                    this.OnNameChanged();
                }
            }
        }

        [Column(Storage = "_Attribute", DbType = "NVarChar(50) NOT NULL", CanBeNull = false)]
        public string Attribute
        {
            get
            {
                return this._Attribute;
            }
            set
            {
                if ((this._Attribute != value))
                {
                    this.OnAttributeChanging(value);
                    this.SendPropertyChanging();
                    this._Attribute = value;
                    this.SendPropertyChanged("Attribute");
                    this.OnAttributeChanged();
                }
            }
        }

        [Column(Storage = "_HasChild", DbType = "Bit NOT NULL")]
        public bool HasChild
        {
            get
            {
                return this._HasChild;
            }
            set
            {
                if ((this._HasChild != value))
                {
                    this.OnHasChildChanging(value);
                    this.SendPropertyChanging();
                    this._HasChild = value;
                    this.SendPropertyChanged("HasChild");
                    this.OnHasChildChanged();
                }
            }
        }

        [Column(Storage = "_IsInheritance", DbType = "Bit NOT NULL")]
        public bool IsInheritance
        {
            get
            {
                return this._IsInheritance;
            }
            set
            {
                if ((this._IsInheritance != value))
                {
                    this.OnIsInheritanceChanging(value);
                    this.SendPropertyChanging();
                    this._IsInheritance = value;
                    this.SendPropertyChanged("IsInheritance");
                    this.OnIsInheritanceChanged();
                }
            }
        }

        [Column(Name = "[Level]", Storage = "_Level", DbType = "Int NOT NULL")]
        public int Level
        {
            get
            {
                return this._Level;
            }
            set
            {
                if ((this._Level != value))
                {
                    this.OnLevelChanging(value);
                    this.SendPropertyChanging();
                    this._Level = value;
                    this.SendPropertyChanged("Level");
                    this.OnLevelChanged();
                }
            }
        }

        [Column(Storage = "_RefObjectID", DbType = "Int")]
        public System.Nullable<int> RefObjectID
        {
            get
            {
                return this._RefObjectID;
            }
            set
            {
                if ((this._RefObjectID != value))
                {
                    this.OnRefObjectIDChanging(value);
                    this.SendPropertyChanging();
                    this._RefObjectID = value;
                    this.SendPropertyChanged("RefObjectID");
                    this.OnRefObjectIDChanged();
                }
            }
        }

        [Column(Storage = "_RefObjectType", DbType = "VarChar(50) NOT NULL", CanBeNull = false)]
        public string RefObjectType
        {
            get
            {
                return this._RefObjectType;
            }
            set
            {
                if ((this._RefObjectType != value))
                {
                    this.OnRefObjectTypeChanging(value);
                    this.SendPropertyChanging();
                    this._RefObjectType = value;
                    this.SendPropertyChanged("RefObjectType");
                    this.OnRefObjectTypeChanged();
                }
            }
        }

        [Column(Storage = "_CreateDate", DbType = "DateTime NOT NULL")]
        public System.DateTime CreateDate
        {
            get
            {
                return this._CreateDate;
            }
            set
            {
                if ((this._CreateDate != value))
                {
                    this.OnCreateDateChanging(value);
                    this.SendPropertyChanging();
                    this._CreateDate = value;
                    this.SendPropertyChanged("CreateDate");
                    this.OnCreateDateChanged();
                }
            }
        }

        [Column(Storage = "_CreatorID", DbType = "Int NOT NULL")]
        public int CreatorID
        {
            get
            {
                return this._CreatorID;
            }
            set
            {
                if ((this._CreatorID != value))
                {
                    this.OnCreatorIDChanging(value);
                    this.SendPropertyChanging();
                    this._CreatorID = value;
                    this.SendPropertyChanged("CreatorID");
                    this.OnCreatorIDChanged();
                }
            }
        }

        [Column(Storage = "_CreatorName", DbType = "NVarChar(50) NOT NULL", CanBeNull = false)]
        public string CreatorName
        {
            get
            {
                return this._CreatorName;
            }
            set
            {
                if ((this._CreatorName != value))
                {
                    this.OnCreatorNameChanging(value);
                    this.SendPropertyChanging();
                    this._CreatorName = value;
                    this.SendPropertyChanged("CreatorName");
                    this.OnCreatorNameChanged();
                }
            }
        }

        [Column(Storage = "_LastModifiedDate", DbType = "DateTime NOT NULL")]
        public System.DateTime LastModifiedDate
        {
            get
            {
                return this._LastModifiedDate;
            }
            set
            {
                if ((this._LastModifiedDate != value))
                {
                    this.OnLastModifiedDateChanging(value);
                    this.SendPropertyChanging();
                    this._LastModifiedDate = value;
                    this.SendPropertyChanged("LastModifiedDate");
                    this.OnLastModifiedDateChanged();
                }
            }
        }

        [Column(Storage = "_LastModifierID", DbType = "Int NOT NULL")]
        public int LastModifierID
        {
            get
            {
                return this._LastModifierID;
            }
            set
            {
                if ((this._LastModifierID != value))
                {
                    this.OnLastModifierIDChanging(value);
                    this.SendPropertyChanging();
                    this._LastModifierID = value;
                    this.SendPropertyChanged("LastModifierID");
                    this.OnLastModifierIDChanged();
                }
            }
        }

        [Column(Storage = "_LastModifierName", DbType = "NVarChar(50) NOT NULL", CanBeNull = false)]
        public string LastModifierName
        {
            get
            {
                return this._LastModifierName;
            }
            set
            {
                if ((this._LastModifierName != value))
                {
                    this.OnLastModifierNameChanging(value);
                    this.SendPropertyChanging();
                    this._LastModifierName = value;
                    this.SendPropertyChanged("LastModifierName");
                    this.OnLastModifierNameChanged();
                }
            }
        }

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void SendPropertyChanging()
        {
            if ((this.PropertyChanging != null))
            {
                this.PropertyChanging(this, emptyChangingEventArgs);
            }
        }

        protected virtual void SendPropertyChanged(String propertyName)
        {
            if ((this.PropertyChanged != null))
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
