using System;
using System.Collections.Generic;
using System.Text;

namespace QTB3.Api.LabResultPatterns.Abstractions
{
    public class LinkRelations
    {
        public const string UomPageRelation = "http://qtb3.com/rel/lrp/uom/page";
        public const string UomReadItemRelation = "http://qtb3.com/rel/lrp/uom/readitem";
        public const string UomWriteItemRelation = "http://qtb3.com/rel/lrp/uom/writeitem";

        public const string PropertyPageRelation = "http://qtb3.com/rel/lrp/property/page";
        public const string PropertyReadItemRelation = "http://qtb3.com/rel/lrp/property/readitem";
        public const string PropertyWriteItemRelation = "http://qtb3.com/rel/lrp/property/writeitem";

        public const string ConstraintPageRelation = "http://qtb3.com/rel/lrp/constraint/page";
        public const string ConstraintReadItemRelation = "http://qtb3.com/rel/lrp/constraint/readitem";
        public const string ConstraintWriteItemRelation = "http://qtb3.com/rel/lrp/constraint/writeitem";

        public const string PatternPageRelation = "http://qtb3.com/rel/lrp/pattern/page";
        public const string PatternReadItemRelation = "http://qtb3.com/rel/lrp/pattern/readitem";
        public const string PatternWriteItemRelation = "http://qtb3.com/rel/lrp/pattern/writeitem";

        public const string PatternConstraintPageRelation = "http://qtb3.com/rel/lrp/pattern/constraint/page";
        public const string PatternConstraintReadItemRelation = "http://qtb3.com/rel/lrp/pattern/constraint/readitem";
        public const string PatternConstraintWriteItemRelation = "http://qtb3.com/rel/lrp/pattern/constraint/writeitem";

    }

    public class LinkTemplates
    {
        public const string UomPageLinkTemplate = "/lrp/uoms?searchText={filter}&skip={skip}&take={take}";
        public const string UomItemLinkTemplate = "/lrp/uoms/{id}";

        public const string PropertyPageLinkTemplate = "/lrp/properties?filter={filter}&skip={skip}&take={take}";
        public const string PropertyItemLinkTemplate = "/lrp/properties/{id}";

        public const string ConstraintPageLinkTemplate = "/lrp/properties/{id}/constraints?searchText={filter}&skip={skip}&take={take}";
        public const string ConstraintItemLinkTemplate = "/lrp/constraints/{id}";

        public const string PatternPageLinkTemplate = "/lrp/patterns?searchText={filter}&skip={skip}&take={take}";
        public const string PatternItemLinkTemplate = "/lrp/patterns/{id}";

        public const string PatternConstraintPageLinkTemplate = "/lrp/patterns/{id}/constraints?searchText={filter}&skip={skip}&take={take}";
        public const string PatternConstraintItemLinkTemplate = "/lrp/pattern-constraint/{id}";

    }
}
