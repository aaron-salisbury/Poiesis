using System;

namespace PoiesisDB.App.Base
{
    // Simple Navigation: rachel53461.wordpress.com/2011/12/18/navigation-with-mvvm-2/
    // I use to populate and update a ContentControl inside of a view.
    public interface IPageViewModel
    {
        string Name { get; }
    }
}
