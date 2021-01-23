using System;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Support.V7.Widget.Helper;
using Android.Views;
using Android.Widget;

namespace RecyclerViewReorder
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, IOnStartDragListener
    {
        private ItemTouchHelper mItemTouchHelper;
        private Switch Switch;
        private SimpleItemTouchHelperCallback itemHelperCallback;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            RecyclerListAdapter adapter = new RecyclerListAdapter(this);

            RecyclerView recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
            recyclerView.HasFixedSize = true;
            recyclerView.SetAdapter(adapter);
            recyclerView.SetLayoutManager(new LinearLayoutManager(this));

            itemHelperCallback = new SimpleItemTouchHelperCallback(adapter);

            ItemTouchHelper.Callback callback = itemHelperCallback;
            mItemTouchHelper = new ItemTouchHelper(callback);
            mItemTouchHelper.AttachToRecyclerView(recyclerView);

            Switch = FindViewById<Switch>(Resource.Id.switch1);
            Switch.CheckedChange += Switch_CheckedChange;

        }

        private void Switch_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            itemHelperCallback.SetItemViewSwipeEnabled(e.IsChecked);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public void OnStartDrag(RecyclerView.ViewHolder viewHolder)
        {
            mItemTouchHelper.StartDrag(viewHolder);
        }
    }
}
