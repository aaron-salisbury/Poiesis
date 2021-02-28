using Poiesis.Core.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Poiesis.Core.Domains
{
    public class DatabaseFacade : ValidatableModel
    {
        private string _name;
        [Required]
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        private string _dataSource;
        [Required]
        [Display(Name = "Data Source")]
        public string DataSource
        {
            get => _dataSource;
            set
            {
                _dataSource = value;
                RaisePropertyChanged();
            }
        }

        public string ConnectionString { get => $"Data Source={DataSource};Initial Catalog={Name};Integrated Security=True;"; }

        private Dictionary<SQLServerSystemFile.Types, SQLServerSystemFile> _sqlServerSystemFilesByTypes;
        public Dictionary<SQLServerSystemFile.Types, SQLServerSystemFile> SQLServerSystemFilesByTypes
        {
            get => _sqlServerSystemFilesByTypes;
            set
            {
                _sqlServerSystemFilesByTypes = value;
                RaisePropertyChanged();
            }
        }
    }
}
