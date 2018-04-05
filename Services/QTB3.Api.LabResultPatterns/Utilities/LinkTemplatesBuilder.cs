
using System.Collections.Generic;
using QTB3.Api.Abstractions.Utilities;
using QTB3.Api.LabResultPatterns.Abstractions;

namespace QTB3.Api.LabResultPatterns.Utilities
{
    public class LinkTemplatesBuilder : ILinkTemplatesBuilder
    {
        public string Build(IUrlBases urlBases)
        {
            var links = new List<string>
                {
                    BuildLinkTemplate(urlBases.ReadUrl, LinkTemplates.UomPageLinkTemplate, LinkRelations.UomPageRelation),
                    BuildLinkTemplate(urlBases.ReadUrl, LinkTemplates.UomItemLinkTemplate, LinkRelations.UomReadItemRelation),
                    BuildLinkTemplate(urlBases.ReadUrl, LinkTemplates.PropertyPageLinkTemplate, LinkRelations.PropertyPageRelation),
                    BuildLinkTemplate(urlBases.ReadUrl, LinkTemplates.PropertyItemLinkTemplate, LinkRelations.PropertyReadItemRelation),
                    BuildLinkTemplate(urlBases.ReadUrl, LinkTemplates.ConstraintPageLinkTemplate, LinkRelations.ConstraintPageRelation),
                    BuildLinkTemplate(urlBases.ReadUrl, LinkTemplates.ConstraintItemLinkTemplate, LinkRelations.ConstraintReadItemRelation),
                    BuildLinkTemplate(urlBases.ReadUrl, LinkTemplates.PatternPageLinkTemplate, LinkRelations.PatternPageRelation),
                    BuildLinkTemplate(urlBases.ReadUrl, LinkTemplates.PatternItemLinkTemplate, LinkRelations.PatternReadItemRelation),
                    BuildLinkTemplate(urlBases.ReadUrl, LinkTemplates.PatternConstraintPageLinkTemplate, LinkRelations.PatternConstraintPageRelation),
                    BuildLinkTemplate(urlBases.ReadUrl, LinkTemplates.PatternConstraintItemLinkTemplate, LinkRelations.PatternConstraintReadItemRelation),

                    BuildLinkTemplate(urlBases.WriteUrl, LinkTemplates.UomItemLinkTemplate, LinkRelations.UomWriteItemRelation),
                    BuildLinkTemplate(urlBases.WriteUrl, LinkTemplates.PropertyItemLinkTemplate, LinkRelations.PropertyWriteItemRelation),
                    BuildLinkTemplate(urlBases.WriteUrl, LinkTemplates.ConstraintItemLinkTemplate, LinkRelations.ConstraintWriteItemRelation),
                    BuildLinkTemplate(urlBases.WriteUrl, LinkTemplates.PatternItemLinkTemplate, LinkRelations.PatternWriteItemRelation),
                    BuildLinkTemplate(urlBases.WriteUrl, LinkTemplates.PatternConstraintItemLinkTemplate, LinkRelations.PatternConstraintWriteItemRelation)
                };
            return string.Join(",", links.ToArray());
        }

        private string BuildLinkTemplate(string urlBase, string urlTemplate, string rel)
        {
            return $"<{urlBase}{urlTemplate}>; rel={rel}";
        }
    }
}
