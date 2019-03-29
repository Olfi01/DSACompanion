using System;
using Android;
using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using DSACompanion.Helpers;
using DSACompanion.Storage;
using Orientation = Android.Widget.Orientation;

namespace DSACompanion
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        private AppStorage Storage { get; set; }
        private FrameLayout mainLayout;
        private View currentView;
        private int diesLayoutId;
        private readonly Random rnd = new Random();
        private const int diesPerRow = 3;
        private AudioManager audioManager;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();

            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);

            mainLayout = FindViewById<FrameLayout>(Resource.Id.main_layout);

            Storage = new AppStorage(this);

            audioManager = (AudioManager)GetSystemService(AudioService);
        }

        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if(drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                base.OnBackPressed();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View) sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        public bool OnNavigationItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.nav_dies)
            {
                SetView(CreateD20sLayout(Storage.NumberOfDies, mainLayout.Width));
            }
            else if (id == Resource.Id.nav_gallery)
            {
                
            }
            else if (id == Resource.Id.nav_slideshow)
            {

            }
            else if (id == Resource.Id.nav_manage)
            {

            }
            else if (id == Resource.Id.nav_share)
            {

            }
            else if (id == Resource.Id.nav_send)
            {

            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

        private void SetView(View fill)
        {
            mainLayout.RemoveAllViews();
            mainLayout.AddView(fill);
            currentView = fill;
        }

        private LinearLayout CreateD20sLayout(int numberOfDies, int parentLayoutWidth)
        {
            LinearLayout layout = new LinearLayout(this)
            {
                Orientation = Orientation.Vertical
            };
            layout.SetGravity(GravityFlags.Center);
            layout.SetForegroundGravity(GravityFlags.Center);

            diesLayoutId = View.GenerateViewId();
            LinearLayout dies = new LinearLayout(this)
            {
                Orientation = Orientation.Vertical,
                Id = diesLayoutId,
            };
            dies.SetGravity(GravityFlags.Center);
            FillWithD20s(numberOfDies, parentLayoutWidth, dies);
            layout.AddView(dies, ViewGroup.LayoutParams.WrapContent);

            LinearLayout controls = new LinearLayout(this)
            {
                Orientation = Orientation.Horizontal
            };
            controls.SetGravity(GravityFlags.Center);
            Spinner spinner = new Spinner(this, SpinnerMode.Dialog);
            spinner.ItemSelected += D20s_Number_Selected;
            var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.die_numbers_array, Android.Resource.Layout.SimpleSpinnerItem);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SelectDialogSingleChoice);
            spinner.Adapter = adapter;
            spinner.SetSelection(numberOfDies - 1);
            controls.AddView(spinner);
            Button button = new Button(this)
            {
                Text = GetString(Resource.String.roll_the_dies)
            };
            button.Click += RollTheD20sButton_Click;
            controls.AddView(button);

            layout.AddView(controls, ViewGroup.LayoutParams.WrapContent);

            return layout;
        }

        private void RollTheD20sButton_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Storage.NumberOfDies; i++)
            {
                RollD20(i);
            }
        }

        private void RollD20(int number)
        {
            MediaPlayer player = MediaPlayer.Create(this, Resource.Raw.roll_dice);
            var volume = audioManager.GetStreamVolume(Stream.Music);
            player.SetVolume(0, volume);
            player.Completion += (sender, e) => player.Release();
            player.Start();
            LinearLayout dies = currentView.FindViewById<LinearLayout>(diesLayoutId);
            LinearLayout row = (LinearLayout)dies.GetChildAt(number / diesPerRow);
            ImageView image = (ImageView)row.GetChildAt(number % diesPerRow);
            image.SetImageDrawable(GetDrawable(R.Dies[rnd.Next(1, 20)]));
        }

        private void FillWithD20s(int numberOfDies, int parentLayoutWidth, LinearLayout dies)
        {
            dies.RemoveAllViews();
            LinearLayout sub = null;
            int imageSize = (parentLayoutWidth / Math.Min(numberOfDies, diesPerRow)) - 5;
            for (int i = 0; i < numberOfDies; i++)
            {
                if (i % diesPerRow == 0)
                {
                    sub = new LinearLayout(this) { Orientation = Orientation.Horizontal };
                    sub.SetGravity(GravityFlags.Center);
                    dies.AddView(sub);
                }
                ImageView image = new ImageView(this);
                image.SetImageDrawable(GetDrawable(R.Dies[1]));
                image.SetForegroundGravity(GravityFlags.Center);
                int diceNr = i;
                image.Click += (sender, e) => RollD20(diceNr);

                sub.AddView(image, width: imageSize, height: imageSize);
            }
        }

        private void D20s_Number_Selected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            var number = int.Parse(spinner.GetItemAtPosition(e.Position).ToString());
            Storage.NumberOfDies = number;
            Storage.SaveChanges();
            ReloadD20sLayout();
        }

        private void ReloadD20sLayout()
        {
            LinearLayout dies = currentView.FindViewById<LinearLayout>(diesLayoutId);
            var numberOfDies = Storage.NumberOfDies;
            var parentLayoutWidth = currentView.Width;
            FillWithD20s(numberOfDies, parentLayoutWidth, dies);
        }
    }
}

