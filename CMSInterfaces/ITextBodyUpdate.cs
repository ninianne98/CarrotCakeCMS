using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Carrotware.CMS.Interface {
	public interface ITextBodyUpdate {

		string UpdateContent(string TextContent);

		string UpdateContentPlainText(string TextContent);

		string UpdateContentRichText(string TextContent);

		string UpdateContentComment(string TextContent);

		string UpdateContentSnippet(string TextContent);

	}
}
