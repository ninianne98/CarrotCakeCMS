﻿using System;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.Core {

	public class WordPressUser {

		public WordPressUser() { }

		public int AuthorId { get; set; }

		public Guid ImportUserID { get; set; }

		public string Login { get; set; }
		public string Email { get; set; }

		public string FirstName { get; set; }
		public string LastName { get; set; }
	}
}