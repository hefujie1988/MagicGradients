﻿using LiteDB;
using Playground.Data.Infrastructure;
using Playground.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Playground.Data.Repositories
{
    public class GradientRepository : IGradientRepository
    {
        private readonly IDatabaseProvider _databaseProvider;
        
        public GradientRepository()
        {
            _databaseProvider = DependencyService.Get<IDatabaseProvider>();
        }
        
        public void UpdateDatabase(LiteDatabase db, Metadata metadata, IDocumentRepository documentRepository)
        {
            var collection = db.GetCollection<Gradient>(nameof(Gradient));

            if (collection.Count() > 0)
            {
                collection.Delete(Query.All());
            }

            var documents = documentRepository.GetDocumentCollection<Gradient>(metadata.NameSpace, metadata.Gradients);

            collection.InsertBulk(documents);
            collection.EnsureIndex(x => x.Tags);
        }

        public Gradient GetById(Guid id)
        {
            using (var db = _databaseProvider.CreateDatabase())
            {
                var collection = db.GetCollection<Gradient>(nameof(Gradient));
                return collection.FindById(new BsonValue(id));
            }
        }

        public IEnumerable<Gradient> GetByTag(string tag)
        {
            using (var db = _databaseProvider.CreateDatabase())
            {
                var collection = db.GetCollection<Gradient>(nameof(Gradient));
                return collection.Find(x => x.Tags.Contains(tag)).ToList();
            }
        }

        public IEnumerable<Gradient> FilterByTags(string category, params string[] tags)
        {
            using (var db = _databaseProvider.CreateDatabase())
            {
                var collection = db.GetCollection<Gradient>(nameof(Gradient));
                return collection
                    .Find(x => x.Tags.Contains(category))
                    .Where(x => x.Tags.Intersect(tags).Any())
                    .ToList();
            }
        }

        public IEnumerable<Gradient> GetPreviewsForTags(string[] tags)
        {
            using (var db = _databaseProvider.CreateDatabase())
            {
                var collection = db.GetCollection<Gradient>(nameof(Gradient));
                var result = new List<Gradient>();

                foreach (var tag in tags)
                {
                    result.Add(collection.FindOne(x => x.Tags.Contains(tag) && x.IsPreview));
                }

                return result;
            }
        }
    }
}
