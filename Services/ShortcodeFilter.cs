using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using Orchard.DisplayManagement;
using Orchard.DisplayManagement.Implementation;
using Orchard.Services;

//https://core.trac.wordpress.org/browser/tags/3.8.1/src/wp-includes/shortcodes.php#L0
namespace NGM.Shortcode.Services {
    public class ShortcodeFilter : IHtmlFilter {
        private readonly IDisplayManager _displayManager;
        private readonly IShapeFactory _shapeFactory;

        public ShortcodeFilter(IDisplayManager displayManager,
            IShapeFactory shapeFactory) {
            _displayManager = displayManager;
            _shapeFactory = shapeFactory;
        }

        //https://core.trac.wordpress.org/browser/tags/3.8.1/src/wp-includes/shortcodes.php
        public string ProcessContent(string text, string flavor) {

            //dynamic foo = factory.Create("Foo", ArgsUtility.Named(new { one = 1, two = "dos" }));

            var args = new RouteValueDictionary(new Dictionary<string, object>{});

            var shape = _shapeFactory.Create("ShortCodeName", Arguments.FromT(args.Values, args.Keys));
            

            var context = new DisplayContext {
                Value = shape,
                ViewContext = new ViewContext()
            };

            var shortCodeText = _displayManager.Execute(context);
        }

	        // WARNING! Do not change this regex without changing do_shortcode_tag() and strip_shortcode_tag()
	        // Also, see shortcode_unautop() and shortcode.js.
        public string GetShortcodeRegex() {
            return
	              "\\["                              // Opening bracket
                + "(\\[?)"                           // 1: Optional second opening bracket for escaping shortcodes: [[tag]]
                //+ "($tagregexp)"                     // 2: Shortcode name
                + "(?![\\w-])"                       // Not followed by word character or hyphen
                + "("                                // 3: Unroll the loop: Inside the opening shortcode tag
                +     "[^\\]\\/]*"                   // Not a closing bracket or forward slash
                +     "(?:"
                +         "\\/(?!\\])"               // A forward slash not followed by a closing bracket
                +         "[^\\]\\/]*"               // Not a closing bracket or forward slash
                +     ")*?"
                + ")"
                + "(?:"
                +     "(\\/)"                        // 4: Self closing tag ...
                +     "\\]"                          // ..+ and closing bracket
                + "|"
                +     "\\]"                          // Closing bracket
                +     "(?:"
                +         "("                        // 5: Unroll the loop: Optionally, anything between the opening and closing shortcode tags
                +             "[^\\[]*+"             // Not an opening bracket
                +             "(?:"
                +                 "\\[(?!\\/\\2\\])" // An opening bracket not followed by the closing shortcode tag
                +                 "[^\\[]*+"         // Not an opening bracket
                +             ")*+"
                +         ")"
                +         "\\[\\/\\2\\]"             // Closing shortcode tag
                +     ")?"
                + ")"
                + "(\\]?)";                          // 6: Optional second closing brocket for escaping shortcodes: [[tag]]
        }
    }
}