﻿using MongoDB.Driver;

namespace projetoBlog.Models
{
    public class AcessoMongoDB
    {
        public const string STRING_DE_CONEXÃO = "mongodb://localhost:27017";
        public const string NOME_DA_BASE = "Blog";
        public const string NOME_DA_COLECAO_PUBLICACAO = "Publicacoes";
        public const string NOME_DA_COLECAO_USUARIO = "Usuarios";

        private static readonly IMongoClient _cliente;
        private static readonly IMongoDatabase _BaseDeDados;

        static AcessoMongoDB()
        {
            _cliente = new MongoClient(STRING_DE_CONEXÃO);
            _BaseDeDados = _cliente.GetDatabase(NOME_DA_BASE);
        }

        public IMongoClient Cliente
        {
            get { return _cliente; }
        }

        public IMongoCollection<Usuario> Usuarios
        {
            get { return _BaseDeDados.GetCollection<Usuario>(NOME_DA_COLECAO_USUARIO); }
        }

        public IMongoCollection<Publicacao> Publicacoes
        {
            get { return _BaseDeDados.GetCollection<Publicacao>(NOME_DA_COLECAO_PUBLICACAO); }
        }

    }
}
