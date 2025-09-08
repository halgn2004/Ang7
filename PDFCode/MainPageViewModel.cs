using Maui.PDFView.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Ang7.PDFCode
{
    internal partial class MainPageViewModel : ObservableObject
    {
       

        [ObservableProperty] private string _pdfSource;
        [ObservableProperty] private bool _isHorizontal;
        [ObservableProperty] private float _maxZoom = 4;
        [ObservableProperty] private string _pagePosition;
        [ObservableProperty] private uint _pageIndex = 0;
        [ObservableProperty] private uint _maxPageIndex = uint.MaxValue;

        [RelayCommand]
        private void Appearing(string st)
        {
            ChangeUri(st);
        }

        [RelayCommand]
        private void ChangeUri(string st)
        {
            PdfSource = st; 
        }

        [RelayCommand]
        private void PageChanged(PageChangedEventArgs args)
        {
            MaxPageIndex = (uint)args.TotalPages - 1;
            PagePosition = $"{args.CurrentPage} - {args.TotalPages}";
            
        }

        [RelayCommand]
        private void PrevPage()
        {
            if (PageIndex > 0)
                PageIndex--;
        }

        [RelayCommand]
        private void NextPage()
        {
            if (PageIndex < MaxPageIndex)
                PageIndex++;
        }
    }
    
}
