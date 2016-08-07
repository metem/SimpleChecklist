﻿using Autofac.Features.Indexed;
using Xamarin.Forms;

namespace SimpleChecklist.Views
{
    public partial class MainView : TabbedPage
    {
        public MainView(IIndex<ViewsId, ContentPage> pages)
        {
            InitializeComponent();

            Children.Add(pages[ViewsId.TaskList]);
            Children.Add(pages[ViewsId.DoneList]);
            Children.Add(pages[ViewsId.Settings]);
            Children.Add(pages[ViewsId.About]);
        }
    }
}