using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Org.Carfh.Courseware.Models.Repositories
{
    public class LanguagesRepository
    {
        private CoursewareDataContext _context;

        // -------------------------------------------------------------------------------------------------------------------------------- //
        // CONSTRUCTOR METHOD                                                                                                               //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public LanguagesRepository(ref CoursewareDataContext context)
        {
            _context = context ?? new CoursewareDataContext();
        } // CLASS CONSTRUCTOR ENDS ------------------------------------------------------------------------------------------------------- //

        // -------------------------------------------------------------------------------------------------------------------------------- //
        // METHOD GET                                                                                                                       //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<Language> Get()
        {
            var languages = new List<Language>();
            try
            {
                var query = from x in _context.Languages
                            select x;
                foreach (var item in query)
                    languages.Add(item);
            } // TRY ENDS
            catch (Exception e)
            {
                Trace.Fail(e.Message);
            }
            return languages;
        } // METHOD GET ENDS ------------------------------------------------------------------------------------------------------------ //

        // -------------------------------------------------------------------------------------------------------------------------------- //
        // METHOD GET                                                                                                                       //
        // -------------------------------------------------------------------------------------------------------------------------------- //
        /// <summary>
        /// 
        /// </summary>
        /// <param name="isoCode"></param>
        /// <returns></returns>
        public Language Get(string isoCode)
        {
            var lang = new Language();
            try
            {
                var query = from x in _context.Languages
                    where x.ISO639a1Code == isoCode
                    select x;
                foreach (var item in query)
                    lang = item;
                if (lang.Id == Guid.Empty)
                    lang = null;
            } // TRY ENDS
            catch (Exception e)
            {
                lang = null;
                Trace.Fail(e.Message);
            } // CATCH ENDS
            return lang;
        }

    }
}
