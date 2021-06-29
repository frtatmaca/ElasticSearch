using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ElasticSerarch.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //CreateNewInex();
            GetIndex();
            return View();
        }

        public void GetIndex()
        {
            var node = new Uri("http://localhost:9200/");
            var settings = new ConnectionSettings(node);

            var client = new ElasticClient(settings);

            //Aşağıdaki sonuçda da görüldüğü gibi hem UserID‘si 2 olan hem de 
            //“PostTest” field’ında “angular” kelimesi geçen rowlar bulunmuştur.
           // var response = client.Search<Post>(p => p
           //  .From(0)
           //  .Size(10)
           //  .Query(q =>
           //             q.Term(f => f.UserID, 2)
           //             || q.MatchPhrasePrefix(mq => mq.Field(f => f.PostTest).Query("angular"))
           //         )
           //);

            var response3 = client.Search<Post>(p => p
             .Query(q => q.Term(t => t.UserID, 1))
             .Query(q => q.MatchPhrasePrefix(mq => mq.Field(f => f.PostTest).Query("angular")))
             .PostFilter(f => f.DateRange(r => r.Field(f2 => f2.PostDate).GreaterThanOrEquals(DateTime.Now.AddDays(-4)))));

           // Index name is null for the given type and no default index is set.Map an index name 
          //using ConnectionSettings.MapDefaultTypeIndices() or set a default index using ConnectionSettings.DefaultIndex().
        }

        public static void CreateNewInex()
        {
            var indexSettings = new IndexSettings();
            indexSettings.NumberOfReplicas = 1;
            indexSettings.NumberOfShards = 3;

            var createIndexDescriptor = new CreateIndexDescriptor("blog_history")
                    .Mappings(ms => ms.Map<Post>(m => m.AutoMap()))
                    .InitializeUsing(new IndexState() { Settings = indexSettings })
                    .Aliases(a => a.Alias("bora_blog"));


            var node = new Uri("http://localhost:9200/");
            var settings = new ConnectionSettings(node);

            //settings.DefaultIndex("defaultindex")
            //    .MapDefaultTypeIndices(m => m.Add(typeof(Post), "my_blog"));


            var client = new ElasticClient(settings);

            //Kaydedilecek Postlar yani Document'ler
            var post = new Post()
            {
                PostDate = DateTime.Now,
                PostTest = "Begining Angular2",
                UserID = 1
            };

            var post2 = new Post()
            {
                PostDate = DateTime.Now,
                PostTest = "What is Elastic Search",
                UserID = 2
            };

            //İlgili Documentlerin Indexlenmesi
            client.Index<Post>(post, idx => idx.Index("my_blog"));
            client.Index<Post>(post2, idx => idx.Index("my_blog"));

            //var response = client.CreateIndex(createIndexDescriptor);
        }
    }
}