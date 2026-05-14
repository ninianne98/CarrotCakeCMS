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

	public interface ITextBodyUpdate {

		string UpdateContent(string TextContent);

		string UpdateContentPlainText(string TextContent);

		string UpdateContentRichText(string TextContent);

		string UpdateContentComment(string TextContent);

		string UpdateContentSnippet(string TextContent);
	}
}