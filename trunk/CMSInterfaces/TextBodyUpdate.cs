using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
