using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.Web.UI.Controls {

	internal class CarrotSortButtonHeaderTemplate : ITemplate {
		private string _cmd { get; set; }
		private string _text { get; set; }
		private string _lnkname { get; set; }
		private int _idx { get; set; }

		public CarrotSortButtonHeaderTemplate(string lnkname, int idx, string text, string cmd) {
			_text = text;
			_cmd = cmd;
			_idx = idx;
			_lnkname = lnkname;
		}

		public CarrotSortButtonHeaderTemplate(string lnkname, string text, string cmd) {
			_text = text;
			_cmd = cmd;
			_idx = -1;
			_lnkname = lnkname;
		}

		public CarrotSortButtonHeaderTemplate(string text, string cmd) {
			_text = text;
			_cmd = cmd;
			_idx = -1;
			_lnkname = "lnkHead";
		}

		public void InstantiateIn(Control container) {
			var x = container.Parent;
			if (!string.IsNullOrEmpty(_cmd)) {
				PlaceHolder lit = new PlaceHolder();
				lit.DataBinding += new EventHandler(litContent_DataBinding);
				container.Controls.Add(lit);
			} else {
				Literal lit = new Literal();
				lit.Text = _text;
				container.Controls.Add(lit);
			}
		}

		private void litContent_DataBinding(object sender, EventArgs e) {
			PlaceHolder ph = (PlaceHolder)sender;

			var lb = new LinkButton();
			lb.CommandName = _cmd;

			if (_idx >= 0) {
				lb.ID = _lnkname + "_" + _idx.ToString();
			} else {
				string[] IDs = ph.ClientID.Split('_');
				lb.ID = _lnkname + "_" + IDs[IDs.Length - 1];
			}
			lb.Text = _text;

			ph.Controls.Add(lb);
		}
	}

	//=========================================

	public class CarrotAutoItemTemplate : ITemplate {
		private string _format { get; set; }
		private string _field { get; set; }

		public CarrotAutoItemTemplate(string fieldParm, string formatPattern) {
			_format = formatPattern;
			_field = fieldParm;
		}

		public void InstantiateIn(Control container) {
			var litContent = new Literal();
			litContent.Text = _field;

			litContent.DataBinding += new EventHandler(litContent_DataBinding);

			container.Controls.Add(litContent);
		}

		private void litContent_DataBinding(object sender, EventArgs e) {
			Literal litContent = (Literal)sender;
			GridViewRow container = (GridViewRow)litContent.NamingContainer;
			try {
				object oValue = DataBinder.Eval(container, "DataItem." + _field);
				litContent.Text = string.Format(_format, oValue);
			} catch {
				litContent.Text = _field;
			}
		}
	}

	//=========================================

	public class CarrotBooleanImage {
		public static string IconResourcePaperclip = "Carrotware.Web.UI.Controls.CarrotGridView.attach.png";
		public static string IconResourceAffirm = "Carrotware.Web.UI.Controls.CarrotGridView.accept.png";
		public static string IconResourceNegative = "Carrotware.Web.UI.Controls.CarrotGridView.cancel.png";
	}

	//=========================================

	public class CarrotBooleanImageItemTemplate : ITemplate {
		private string _imageTrue { get; set; }
		private string _imageFalse { get; set; }
		private string _defaultImageTrue { get; set; }
		private string _defaultImageFalse { get; set; }

		private string _verbiageTrue { get; set; }
		private string _verbiageFalse { get; set; }
		private string _field { get; set; }
		private string _css { get; set; }

		private bool _designTime = false;
		private bool _altItem = false;
		private int _row = -1;

		public CarrotBooleanImageItemTemplate(CarrotHeaderSortTemplateField field) {
			_designTime = false;
			SetImage();

			_field = field.DataField;
			_css = field.BooleanImageCssClass;

			SetVerbiage();
		}

		public CarrotBooleanImageItemTemplate(CarrotHeaderSortTemplateField field, int rowNbr, bool altItem) : this(field) {
			// used  only with design time!
			_designTime = true;
			_altItem = altItem;
			_row = rowNbr;

			SetImage();

			_imageTrue = field.ImagePathTrue.Length > 1 ? field.ImagePathTrue : _defaultImageTrue;
			_imageFalse = field.ImagePathFalse.Length > 1 ? field.ImagePathFalse : _defaultImageFalse;

			_verbiageTrue = field.AlternateTextTrue;
			_verbiageFalse = field.AlternateTextFalse;
		}

		public void SetImage() {
			_defaultImageTrue = WebControlHelper.GetWebResourceUrl(CarrotBooleanImage.IconResourceAffirm);
			_defaultImageFalse = WebControlHelper.GetWebResourceUrl(CarrotBooleanImage.IconResourceNegative);

			if (_designTime) {
				try {
					// this is because the designer can't actually get URLs to resources, but file paths do work
					var tmp = Environment.GetEnvironmentVariable("TEMP");
					_defaultImageTrue = Path.Combine(tmp, CarrotBooleanImage.IconResourceAffirm);
					_defaultImageFalse = Path.Combine(tmp, CarrotBooleanImage.IconResourceNegative);

					if (!File.Exists(_defaultImageTrue) || File.GetLastWriteTime(_defaultImageTrue) < DateTime.Now.AddMinutes(-1)) {
						var affirm = WebControlHelper.GetManifestResourceBinary(CarrotBooleanImage.IconResourceAffirm);
						var neg = WebControlHelper.GetManifestResourceBinary(CarrotBooleanImage.IconResourceNegative);

						File.WriteAllBytes(_defaultImageTrue, affirm);
						File.WriteAllBytes(_defaultImageFalse, neg);
					}
				} catch (Exception ex) { }
			}

			_imageTrue = _defaultImageTrue;
			_imageFalse = _defaultImageFalse;
		}

		public void SetVerbiage(string imageTextTrue, string imageTextFalse) {
			_verbiageTrue = imageTextTrue;
			_verbiageFalse = imageTextFalse;
		}

		public void SetImage(string imageNameTrue, string imageNameFalse) {
			_imageTrue = imageNameTrue;
			_imageFalse = imageNameFalse;
		}

		public void SetVerbiage() {
			_verbiageTrue = "Active";
			_verbiageFalse = "Inactive";
		}

		public void InstantiateIn(Control container) {
			var imgBool = new Image();
			container.Controls.Add(imgBool);

			if (!string.IsNullOrEmpty(_css)) {
				imgBool.CssClass = _css;
			}

			imgBool.AlternateText = _field;
			imgBool.DataBinding += new EventHandler(imgBool_DataBinding);

			if (_designTime) {
				imgBool.ImageUrl = _altItem ? _imageTrue : _imageFalse;
				imgBool.AlternateText = _altItem ? _verbiageTrue : _verbiageFalse;
			}
		}

		private void imgBool_DataBinding(object sender, EventArgs e) {
			Image imgBool = (Image)sender;
			GridViewRow container = (GridViewRow)imgBool.NamingContainer;

			imgBool.ImageUrl = WebControlHelper.GetWebResourceUrl(CarrotBooleanImage.IconResourcePaperclip);

			try {
				bool itemValue = Convert.ToBoolean(DataBinder.Eval(container, "DataItem." + _field).ToString());

				if (itemValue) {
					imgBool.ImageUrl = _imageTrue;
					imgBool.AlternateText = _verbiageTrue;
				} else {
					imgBool.ImageUrl = _imageFalse;
					imgBool.AlternateText = _verbiageFalse;
				}
				imgBool.ToolTip = imgBool.AlternateText;
			} catch {
				imgBool.AlternateText = _field;
				imgBool.ToolTip = _field;
			}
		}
	}

	//=========================================

	public class CarrotImageItemTemplate : ITemplate {
		private List<CarrotImageColumnData> _images { get; set; }
		private string _field { get; set; }
		private string _css { get; set; }

		private bool _designTime = false;
		private bool _altItem = false;
		private int _row = -1;

		public CarrotImageItemTemplate(CarrotHeaderSortTemplateField field) {
			_designTime = false;
			_field = field.DataField;
			_css = field.BooleanImageCssClass;

			if (field.ImageSelectors != null) {
				_images = field.ImageSelectors;
			} else {
				_images = new List<CarrotImageColumnData>();
			}
		}

		public CarrotImageItemTemplate(CarrotHeaderSortTemplateField field, int rowNbr, bool altItem) : this(field) {
			// used  only with design time!
			_designTime = true;
			_altItem = altItem;
			_row = rowNbr;
		}

		public void InstantiateIn(Control container) {
			var imgEnum = new Image();
			container.Controls.Add(imgEnum);

			if (!string.IsNullOrEmpty(_css)) {
				imgEnum.CssClass = _css;
			}

			imgEnum.AlternateText = _field;
			imgEnum.DataBinding += new EventHandler(imgEnum_DataBinding);

			if (_designTime) {
				try {
					// this is because the designer can't actually get URLs to resources, but file paths do work
					var tmp = Environment.GetEnvironmentVariable("TEMP");
					var defaultFile = Path.Combine(tmp, CarrotBooleanImage.IconResourcePaperclip);

					if (!File.Exists(defaultFile) || File.GetLastWriteTime(defaultFile) < DateTime.Now.AddMinutes(-1)) {
						var paperclip = WebControlHelper.GetManifestResourceBinary(CarrotBooleanImage.IconResourcePaperclip);
						File.WriteAllBytes(defaultFile, paperclip);
					}

					if (!_images.Any()) {
						_images.Add(new CarrotImageColumnData(string.Empty, "Default", defaultFile));
					}

					imgEnum.ImageUrl = defaultFile;
					// for designer, show as many different of the images as possible
					if (_images.Count > 0) {
						var idx = _row % _images.Count;
						imgEnum.ImageUrl = _images[idx].ImagePath;
						imgEnum.AlternateText = _images[idx].ImageAltText;
					}
				} catch (Exception ex) { }
			}
		}

		private void imgEnum_DataBinding(object sender, EventArgs e) {
			Image imgEnum = (Image)sender;
			GridViewRow container = (GridViewRow)imgEnum.NamingContainer;

			imgEnum.ImageUrl = WebControlHelper.GetWebResourceUrl(CarrotBooleanImage.IconResourcePaperclip);

			try {
				string itemValue = DataBinder.Eval(container, "DataItem." + _field).ToString();

				var icd = (from img in _images
						   where img.KeyValue.ToLowerInvariant() == itemValue.ToLowerInvariant()
						   select img).FirstOrDefault();

				if (icd != null) {
					imgEnum.ImageUrl = icd.ImagePath;
					imgEnum.AlternateText = icd.ImageAltText;
				} else {
					imgEnum.AlternateText = "[" + _field + "] IMAGE DEF MISSING";
				}
				imgEnum.ToolTip = imgEnum.AlternateText;
			} catch {
				imgEnum.AlternateText = _field;
				imgEnum.ToolTip = _field;
			}
		}
	}
}