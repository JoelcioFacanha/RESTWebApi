using AutoMapper;
using DevIO.Api.ViewModels;
using DevIO.Business.Intefaces.Services;
using DevIO.Business.Interfaces;
using DevIO.Business.Interfaces.Repositories;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DevIO.Api.Controllers
{
    [Route("api/produtos")]
    public class ProdutosController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;
        //private readonly INotificador _notificador;

        public ProdutosController(IProdutoRepository produtoRepository,
                                  IProdutoService produtoService,
                                  IMapper mapper,
                                  INotificador notificador) : base(notificador)
        {
            _produtoRepository = produtoRepository;
            _produtoService = produtoService;
            _mapper = mapper;
            // _notificador = notificador;
        }

        [HttpGet("")]
        public async Task<IEnumerable<ProdutoViewModel>> ObeterTodos()
        {
            return _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosFornecedores());
        }

        // GET api/<ProdutosController>/5
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> ObterProdutoPorId(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null) return NotFound();

            return produtoViewModel;
        }

        // POST api/<ProdutosController>
        [HttpPost]
        public async Task<ActionResult<ProdutoViewModel>> Adicionar(ProdutoViewModel produtoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            #region CODIGO PARA CRIAR AQUIVO FISICO NA PASTA WWWWROOT
            //var imagemNome = Guid.NewGuid() + "_" + produtoViewModel.Imagem;

            //if (!UploadArquivo(produtoViewModel.ImagemUpload, imagemNome))
            //{
            //    return CustomResponse(produtoViewModel);
            //}

            //produtoViewModel.Imagem = imagemNome;
            #endregion

            if (!UploadArquivo(produtoViewModel))
            {
                return CustomResponse(produtoViewModel);
            }

            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return CustomResponse(produtoViewModel);
        }

        // PUT api/<ProdutosController>/5
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> Atualizar(Guid id, ProdutoViewModel produtoViewModel)
        {
            if (id != produtoViewModel.Id)
            {
                NotificarError("O código identificador não confere com o id enviado na url");
                return CustomResponse();
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _produtoService.Atualizar(_mapper.Map<Produto>(produtoViewModel));

            return CustomResponse(produtoViewModel);
        }

        // DELETE api/<ProdutosController>/5
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> Excluir(Guid id)
        {
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null) return NotFound();

            await _produtoService.Remover(id);

            return CustomResponse(produtoViewModel);
        }

        private bool UploadArquivo(string arquivo, string imgNome)
        {
            if (string.IsNullOrEmpty(arquivo))
            {
                NotificarError("Forneça uma imagem para este produto!");
                return false;
            }

            var imagemDataByteArray = Convert.FromBase64String(arquivo);

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgNome);

            if (System.IO.File.Exists(filePath))
            {
                NotificarError("Já existe um arquivo com este nome!");
                return false;
            }

            System.IO.File.WriteAllBytes(filePath, imagemDataByteArray);
            return true;
        }

        private bool UploadArquivo(ProdutoViewModel produtoViewModel)
        {
            if (string.IsNullOrEmpty(produtoViewModel.ImagemUpload))
            {
                NotificarError("Forneça uma imagem para este produto!");
                return false;
            }

            produtoViewModel.Imagem = Convert.FromBase64String(produtoViewModel.ImagemUpload).AsMemory().ToArray();
            return true;
        }

        private async Task<ProdutoViewModel> ObterProduto(Guid id)
        {
            return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));
        }
    }
}
