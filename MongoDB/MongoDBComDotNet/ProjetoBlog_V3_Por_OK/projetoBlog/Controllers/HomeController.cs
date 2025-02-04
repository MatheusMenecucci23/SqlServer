﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using MongoDB.Driver;
using projetoBlog.Models;
using projetoBlog.Models.Home;
using System.Linq.Expressions;
using MongoDB.Bson;

namespace projetoBlog.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            var conectandoMongoDB = new AcessoMongoDB();

            var Filtro = new BsonDocument();
            var PublicacoesRecentes = await conectandoMongoDB.Publicacoes.Find(Filtro).SortByDescending(x => x.DataCriacao).Limit(10).ToListAsync();

            //XXX TRABALHE AQUI
            // liste as dez mais recentes publicações
            // Descomente as linhas abaixo
            var model = new IndexModel
            {
                PublicacoesRecentes = PublicacoesRecentes
            };

            return View(model);
        }

        [HttpGet]
        public ActionResult NovaPublicacao()
        {
            return View(new NovaPublicacaoModel());
        }

        [HttpPost]
        public async Task<ActionResult> NovaPublicacao(NovaPublicacaoModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var post = new Publicacao
            {
                Autor = User.Identity.Name,
                Titulo = model.Titulo,
                Conteudo = model.Conteudo,
                Tags = model.Tags.Split('', ',', ';'),
                DataCriacao = DateTime.UtcNow,
                Comentarios = new List<Comentario>()

            };

            var conectandoMongoDB = new AcessoMongoDB();
            await conectandoMongoDB.Publicacoes.InsertOneAsync(post);

            //XXX TRABALHE AQUI
            // Inclua a publicação na base de dados
            // Descomete a linha abaixo
            return RedirectToAction("Publicacao", new { id = post.Id });
        }

        [HttpGet]
        public async Task<ActionResult> Publicacao(string id)
        {
            var conectandoMongoDB = new AcessoMongoDB();
            var publicacao = await conectandoMongoDB.Publicacoes.Find(x => x.Id == id).SingleOrDefaultAsync();

            //XXX TRABALHE AQUI
            // Busque na base MongoDB a publicação pelo ID
            // Descomente as linhas abaixo
            if (publicacao == null)
            {
                return RedirectToAction("Index");
            }

            var model = new PublicacaoModel
            {
                Publicacao = publicacao,
                NovoComentario = new NovoComentarioModel
                {
                    PublicacaoId = id
                }
            };

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> Publicacoes(string tag = null)
        {
            var conectandoMongoDB = new AcessoMongoDB();
            var posts = new List<Publicacao>();

            if (tag == null)
            {
                var filtro = new BsonDocument();
                posts = await conectandoMongoDB.Publicacoes.Find(filtro).SortByDescending(x => x.DataCriacao).Limit(10).ToListAsync();
            }
            else
            {
                var construtor = Builders<Publicacao>.Filter;
                var condicao = construtor.AnyEq(x => x.Tags, tag);
                posts = await conectandoMongoDB.Publicacoes.Find(condicao).SortByDescending(x => x.DataCriacao).Limit(10).ToListAsync();

            }

            // XXX TRABALHE AQUI
            // Busque as publicações pela TAG escolhida.

            return View(posts);
        }

        [HttpPost]
        public async Task<ActionResult> NovoComentario(NovoComentarioModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Publicacao", new { id = model.PublicacaoId });
            }

            var comment = new Comentario
            {
                Autor = User.Identity.Name,
                Conteudo = model.Conteudo,
                DataCriacao = DateTime.UtcNow
            };

            var connectandoMongoDB = new AcessoMongoDB();

            var construtor = Builders<Publicacao>.Filter;
            var condicao = construtor.Eq(x => x.Id, model.PublicacaoId);
            var construtorAlteracao = Builders<Publicacao>.Update;
            var condicaoAlteracao = construtorAlteracao.Push(x => x.Comentarios, comment);
            await connectandoMongoDB.Publicacoes.UpdateOneAsync(condicao, condicaoAlteracao);

            //XXX TRABALHE AQUI
            // Inclua novo comentário na publicação já existente.

            return RedirectToAction("Publicacao", new { id = model.PublicacaoId });
        }
    }
}