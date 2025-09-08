using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using PanCardView.Extensions;


namespace Ang7.ViewModels;

public sealed class CarouselHomePageViewModel : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private int _currentIndex;
		private int _imageCount = 1078;

		public CarouselHomePageViewModel()
		{
        Items = new ObservableCollection<object>
		{
		    new  { Source = "homepagecarousel2.png", Ind = _imageCount++, Color = Colors.Transparent, Title = "دروس تفاعلية ومتنوعة" , Desc = "مجموعة واسعة من الدروس التي تشمل مقاطع الفيديو والاختبارات التفاعلية لتعزيز تجربة التعلم." , MaxLines=3},
		    new  { Source = "homepagecarousel1.png", Ind = _imageCount++, Color = Colors.Transparent, Title = "واجهة سهلة الاستخدام" , Desc = "تصميم بسيط وبديهي يسمح للمتعلمين بالتنقل بسهولة والوصول إلى الموارد التعليمية بسرعة." , MaxLines=3},
		    new  { Source = "homepagecarousel3.png", Ind = _imageCount++, Color = Colors.Transparent, Title = "محتوى تعليمي محدث" , Desc = "حديث مستمر للمحتوى التعليمي لضمان توفير أحدث المعلومات والمستجدات في مختلف المجالات." , MaxLines=3},
		    new  { Source = "homepagecarousel4.png", Ind = _imageCount++, Color = Colors.Transparent, Title = "دعم فني متواصل" , Desc = "فريق دعم فني جاهز على مدار الساعة لمساعدة المتعلمين في حل أي مشكلات تقنية قد يواجهونها." , MaxLines=3}
		};

        PanPositionChangedCommand = new Command(v =>
			{
				if (IsAutoAnimationRunning || IsUserInteractionRunning)
				{
					return;
				}

				var index = CurrentIndex + (bool.Parse(v.ToString()) ? 1 : -1);
				if (index < 0 || index >= Items.Count)
				{
					return;
				}
				CurrentIndex = index;
			});

			RemoveCurrentItemCommand = new Command(() =>
			{
				if (!Items.Any())
				{
					return;
				}
            Items.RemoveAt(CurrentIndex.ToCyclicalIndex(Items.Count));
			});

			GoToLastCommand = new Command(() =>
			{
				CurrentIndex = Items.Count - 1;
			});
		}

		public ICommand PanPositionChangedCommand { get; }

		public ICommand RemoveCurrentItemCommand { get; }

		public ICommand GoToLastCommand { get; }

		public int CurrentIndex
		{
			get => _currentIndex;
			set
			{
				_currentIndex = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentIndex)));
			}
		}

		public bool IsAutoAnimationRunning { get; set; }

		public bool IsUserInteractionRunning { get; set; }

		public ObservableCollection<object> Items { get; }

	}
