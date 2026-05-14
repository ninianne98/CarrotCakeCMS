/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, 2026, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011, May 2026
*/

namespace Carrotware.CMS.Interface {

	public abstract class TextBodyUpdate : ITextBodyUpdate {

		public virtual string UpdateContent(string TextContent) {
			return TextContent;
		}

		public virtual string UpdateContentPlainText(string TextContent) {
			return TextContent;
		}

		public virtual string UpdateContentRichText(string TextContent) {
			return TextContent;
		}

		public virtual string UpdateContentComment(string TextContent) {
			return TextContent;
		}

		public virtual string UpdateContentSnippet(string TextContent) {
			return TextContent;
		}
	}
}