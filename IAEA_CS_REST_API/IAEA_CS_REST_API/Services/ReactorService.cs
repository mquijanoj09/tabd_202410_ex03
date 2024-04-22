﻿using System;
using IAEA_CS_REST_API.Helpers;
using IAEA_CS_REST_API.Interfaces;
using IAEA_CS_REST_API.Models;
using System.Runtime.Intrinsics.X86;

namespace IAEA_CS_REST_API.Services
{
    public class ReactorService(IReactorRepository reactorRepository)
    {
        private readonly IReactorRepository _reactorRepository = reactorRepository;

        public async Task<IEnumerable<Reactor>> GetAllAsync()
        {
            return await _reactorRepository
                .GetAllAsync();
        }

        public async Task<Reactor> GetByIdAsync(int reactor_id)
        {
            Reactor unaReactor = await _reactorRepository
                .GetByIdAsync(reactor_id);

            if (unaReactor.Id == 0)
                throw new AppValidationException($"Reactor no encontrada con el id {reactor_id}");

            return unaReactor;
        }

        public async Task<Reactor> CreateAsync(Reactor unaReactor)
        {
            //Validamos que la reactor tenga nombre
            if (unaReactor.Nombre!.Length == 0)
                throw new AppValidationException("No se puede insertar una reactor con nombre nulo");

            //Validamos que la reactor tenga url_wikipedia
            if (unaReactor.Url_Wikipedia!.Length == 0)
                throw new AppValidationException("No se puede insertar una reactor con Url de Wikipedia nulo");

            //Validamos que la reactor tenga url_imagen
            if (unaReactor.Url_Imagen!.Length == 0)
                throw new AppValidationException("No se puede insertar una reactor con Url de la imagen nulo");

            //Validamos que no exista previamente una reactor con ese nombre
            var reactorExistente = await _reactorRepository
                .GetByNameAsync(unaReactor.Nombre);

            if (reactorExistente.Id != 0)
                throw new AppValidationException($"Ya existe la reactor {unaReactor.Nombre} ");

            try
            {
                bool resultadoAccion = await _reactorRepository
                    .CreateAsync(unaReactor);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                reactorExistente = await _reactorRepository
                    .GetByNameAsync(unaReactor.Nombre);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return (reactorExistente);
        }

        public async Task<Reactor> UpdateAsync(Reactor unaReactor)
        {
            //Validamos que la reactor tenga Id
            if (unaReactor.Id == 0)
                throw new AppValidationException("El Id de reactor se requiere especificar para realizar actualización");

            //Validamos que la reactor tenga nombre
            if (unaReactor.Nombre!.Length == 0)
                throw new AppValidationException("No se puede actualizar una reactor con nombre nulo");

            //Validamos que la reactor tenga url_wikipedia
            if (unaReactor.Url_Wikipedia!.Length == 0)
                throw new AppValidationException("No se puede actualizar una reactor con Url de Wikipedia nulo");

            //Validamos que la reactor tenga url_imagen
            if (unaReactor.Url_Imagen!.Length == 0)
                throw new AppValidationException("No se puede actualizar una reactor con Url de la imagen nulo");

            //Que la reactor exista con ese Id:            
            var reactorExistente = await _reactorRepository
                .GetByIdAsync(unaReactor.Id);

            if (reactorExistente.Id == 0)
                throw new AppValidationException($"No existe la reactor {unaReactor.Nombre} para actualizar");

            //Que el nombre nuevo de la reactor no exista en otra reactor distinta
            reactorExistente = await _reactorRepository
                .GetByNameAsync(unaReactor.Nombre!);

            if (reactorExistente.Id != unaReactor.Id)
                throw new AppValidationException($"Ya existe la reactor {reactorExistente.Nombre} con el Id {reactorExistente.Id}");

            try
            {
                bool resultadoAccion = await _reactorRepository
                    .UpdateAsync(unaReactor);

                if (!resultadoAccion)
                    throw new AppValidationException("Operación ejecutada pero no generó cambios en la DB");

                reactorExistente = await _reactorRepository
                    .GetByIdAsync(unaReactor.Id);
            }
            catch (DbOperationException)
            {
                throw;
            }

            return (reactorExistente);
        }
    }
}
