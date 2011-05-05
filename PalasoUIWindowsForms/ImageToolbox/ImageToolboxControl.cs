﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Palaso.UI.WindowsForms.ImageToolbox.Cropping;
#if !MONO

#endif

namespace Palaso.UI.WindowsForms.ImageToolbox
{
	public partial class ImageToolboxControl : UserControl
	{
		private readonly ImageList _toolImages;
		private Control _currentControl;
		private PalasoImage _imageInfo;

		public ImageToolboxControl()
		{
			InitializeComponent();

			ImageInfo = new PalasoImage();
			_toolListView.Items.Clear();


			//doing our own image list because VS2010 croaks their resx if have an imagelist while set to .net 3.5 with x86 on a 64bit os (something like that). This is a known bug MS doesn't plan to fix.
			_toolImages = new ImageList();
			_toolListView.LargeImageList = _toolImages;
			_toolImages.ColorDepth = ColorDepth.Depth24Bit;
			_toolImages.ImageSize = new Size(32,32);


			 AddControl("Get Picture", ImageToolboxButtons.browse, "browse",  (x) => new AcquireImageControl());
			  AddControl("Crop",  ImageToolboxButtons.crop, "crop", (x) => new ImageCropper());

			_toolListView.Items[0].Selected = true;
			_toolListView.Refresh();

#if !DEBUG
			_imageMetadataControl.Visible = false;  // until it actuall works
#endif
		}

		/// <summary>
		/// This is the main input/output of this dialog
		/// </summary>
		public PalasoImage ImageInfo
		{
			get { return _imageInfo; }
			set
			{
				try
				{
					if (value == null || value.Image == null)
					{
						_currentImageBox.Image = null;
					}
					else
					{
						if(value.Image == _currentImageBox.Image)
						{
							return;
						}
						/* this seemed like a good idea, but it lead to "parameter errors" later in the image
						 * try
												{
													if (_currentImageBox.Image != null)
													{
														  _currentImageBox.Image.Dispose();
													}
												}
												catch (Exception)
												{
													//ah well. I haven't got a way to know when it's disposable and when it isn't
													throw;
												}
						  */
						_currentImageBox.Image = value.Image;
					}
					_imageInfo = value;
					_imageMetadataControl.SetImage(_imageInfo);
				}
				catch (Exception e)
				{
					Palaso.Reporting.ErrorReport.NotifyUserOfProblem(e, "Sorry, something went wrong while getting the image.");
				}
			}
		}


		private void AddControl(string label, Bitmap bitmap, string imageKey, System.Func<PalasoImage, Control> makeControl)
		{
			_toolImages.Images.Add(bitmap);
			_toolImages.Images.SetKeyName(_toolImages.Images.Count - 1, imageKey);

			var item= new ListViewItem(label);
			item.ImageKey = imageKey;
			item.Tag = makeControl;
			this._toolListView.Items.Add(item);
		}

		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			try
			{

			if (_toolListView.SelectedItems.Count == 0)
				return;
			if(_currentControl !=null)
			{
				GetImageFromCurrentControl();

				_panelForControls.Controls.Remove(_currentControl);
				((IImageToolboxControl)_currentControl).ImageChanged -= new EventHandler(imageToolboxControl_ImageChanged);
				_currentControl.Dispose();
			}
			System.Func<PalasoImage, Control> fun =
				(System.Func<PalasoImage, Control>) _toolListView.SelectedItems[0].Tag;
			_currentControl = fun(ImageInfo);

			_currentControl.Dock = DockStyle.Fill;
			_panelForControls.Controls.Add(_currentControl);

			IImageToolboxControl imageToolboxControl = ((IImageToolboxControl)_currentControl);
			imageToolboxControl.SetImage(ImageInfo);
			imageToolboxControl.ImageChanged += new EventHandler(imageToolboxControl_ImageChanged);
			Refresh();


			}
			catch (Exception error)
			{
				Palaso.Reporting.ErrorReport.NotifyUserOfProblem(error,
																 "Sorry, something went wrong with the ImageToolbox");
			}
		}

		private void GetImageFromCurrentControl()
		{
			ImageInfo = ((IImageToolboxControl) _currentControl).GetImage();
			if (ImageInfo == null)
				_currentImageBox.Image = null;
			else
				_currentImageBox.Image = ImageInfo.Image;
		}

		void imageToolboxControl_ImageChanged(object sender, EventArgs e)
		{
			GetImageFromCurrentControl();
		}

		public void Closing()
		{
			if (_currentControl == null)
			{

			}
			else
			{
				ImageInfo = ((IImageToolboxControl)_currentControl).GetImage();
				Controls.Remove(_currentControl);
				_currentControl.Dispose();
			}

		}
	}

	public interface IImageToolboxControl
	{
		void SetImage(PalasoImage image);
		PalasoImage GetImage();
		event EventHandler ImageChanged;
	}
}
