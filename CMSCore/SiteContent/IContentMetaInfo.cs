﻿using System;

namespace Carrotware.CMS.Core {
	public interface IContentMetaInfo {
		Guid ContentMetaInfoID { get; }
		string MetaInfoText { get; }
		string MetaInfoURL { get; }
		int MetaInfoCount { get; }
		bool MetaIsPublic { get; }

		void SetValue(Guid ContentMetaInfoID);

	}
}
