using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WeakEventViewModelBaseTestApp.Models;
using MakCraft.ViewModels;

namespace WeakEventViewModelBaseTestApp.Services
{
    public sealed class ItemService : NotifyObject, IItemService
    {
        private ItemService() // 外部からのインスタンス生成を抑止
        {
            SampleModels.Add(new SampleModel
            {
                Id = 0,
                Created = DateTime.Now,
                Text = "Sample text."
            });
        }

        private static class SingletonHolder
        {
            internal static ItemService Instance = new ItemService();
        }

        public static ItemService GetInstance()
        {
            return SingletonHolder.Instance;
        }

        public void Add(SampleModel sampleModel)
        {
            var id = SampleModels.Max(s => s.Id);
            sampleModel.Id = ++id;
            SampleModels.Add(sampleModel);
        }

        public void Update(SampleModel sampleModel)
        {
            var target = SampleModels.FirstOrDefault(p => p.Id == sampleModel.Id);
            if (target == null)
            {
                throw new KeyNotFoundException($"Key: {sampleModel.Id}");
            }
            target.Created = sampleModel.Created;
            target.Text = sampleModel.Text;
        }

        private string _item01;
        public string Item01
        {
            get => _item01;
            set => base.SetProperty(ref _item01, value);
        }

        private string _item02;
        public string Item02
        {
            get => _item02;
            set => base.SetProperty(ref _item02, value);
        }
        public ObservableCollection<SampleModel> SampleModels { get; } = new ObservableCollection<SampleModel>();
    }
}
