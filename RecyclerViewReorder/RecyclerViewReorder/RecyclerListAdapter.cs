using System;
using Android.Support.V7.Widget;
using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using Android.Graphics;
using System.Linq;
using Java.Util;
using System.Collections.ObjectModel;
using Android.Support.V4.View;

namespace RecyclerViewReorder
{
    /// <summary>
    /// Interface to listen for a move or dismissal event from a ItemTouchHelper.Callback.
    /// </summary>
    public interface IItemTouchHelperAdapter
    {
        /// <summary>
        /// Called when an item has been dragged far enough to trigger a move. This is called every time
        /// an item is shifted, and <strong>not</strong> at the end of a "drop" event.<br/>
        /// <br/>
        /// Implementations should call {@link RecyclerView.Adapter#notifyItemMoved(int, int)} after
        /// adjusting the underlying data to reflect this move.
        /// </summary>
        /// <returns>True if the item was moved to the new adapter position.</returns>
        /// <param name="fromPosition">The start position of the moved item.</param>
        /// <param name="toPosition">Then resolved position of the moved item.</param>
        bool OnItemMove(int fromPosition, int toPosition);

        /// <summary>
        /// Called when an item has been dismissed by a swipe.<br/>
        /// <br/>
        /// Implementations should call RecyclerView.Adapter#notifyItemRemoved(int) after
        /// adjusting the underlying data to reflect this removal.
        /// </summary>
        /// <param name="position">The position of the item dismissed.</param>
        void OnItemDismiss(int position);
    }

    public class RecyclerListAdapter : RecyclerView.Adapter, IItemTouchHelperAdapter
    {
        private static string[] STRINGS = new String[]{
            "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten"
        };

        private ObservableCollection<string> mItems = new ObservableCollection<string>();

        private IOnStartDragListener mDragStartListener;

        public RecyclerListAdapter(IOnStartDragListener dragStartListener)
        {
            mDragStartListener = dragStartListener;

            foreach (var item in STRINGS)
            {
                mItems.Add(item);
            }
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.item_main, parent, false);
            ItemViewHolder itemViewHolder = new ItemViewHolder(view);
            return itemViewHolder;
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var itemHolder = (ItemViewHolder)holder;

            itemHolder.textView.Text = mItems.ElementAt(position);
            itemHolder.handleView.SetOnTouchListener(new TouchListenerHelper(itemHolder, mDragStartListener));
        }

        public void OnItemDismiss(int position)
        {
            mItems.Remove(mItems.ElementAt(position));
            NotifyItemRemoved(position);
        }

        public bool OnItemMove(int fromPosition, int toPosition)
        {
            mItems.Move(fromPosition, toPosition);
            NotifyItemMoved(fromPosition, toPosition);
            return true;
        }

        public override int ItemCount
        {
            get
            {
                return mItems.Count;
            }
        }

        /// <summary>
        /// Simple example of a view holder that implements ItemTouchHelperViewHolder and has a
        /// "handle" view that initiates a drag event when touched.
        /// </summary>
        public class ItemViewHolder : RecyclerView.ViewHolder, IItemTouchHelperViewHolder
        {

            public TextView textView;
            public ImageView handleView;
            public View _itemView;

            public ItemViewHolder(View itemView) : base(itemView)
            {
                _itemView = itemView;
                textView = itemView.FindViewById<TextView>(Resource.Id.text);
                handleView = itemView.FindViewById<ImageView>(Resource.Id.handle);
            }

            public void OnItemSelected()
            {
                _itemView.SetBackgroundColor(Color.LightGray);
            }

            public void OnItemClear()
            {
                _itemView.SetBackgroundColor(Color.White);
            }
        }
    }
}
