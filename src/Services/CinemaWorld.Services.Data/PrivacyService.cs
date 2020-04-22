﻿namespace CinemaWorld.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using CinemaWorld.Data.Common.Repositories;
    using CinemaWorld.Data.Models;
    using CinemaWorld.Models.InputModels.AdministratorInputModels.Privacy;
    using CinemaWorld.Models.ViewModels.Privacy;
    using CinemaWorld.Services.Data.Common;
    using CinemaWorld.Services.Data.Contracts;
    using CinemaWorld.Services.Mapping;

    using Microsoft.EntityFrameworkCore;

    public class PrivacyService : IPrivacyService
    {
        private readonly IDeletableEntityRepository<Privacy> privacyRepository;

        public PrivacyService(IDeletableEntityRepository<Privacy> privacyRepository)
        {
            this.privacyRepository = privacyRepository;
        }

        public async Task<PrivacyDetailsViewModel> CreateAsync(PrivacyCreateInputModel privacyCreateInputModel)
        {
            var privacy = new Privacy
            {
                PageContent = privacyCreateInputModel.PageContent,
            };

            bool doesPrivacyExist = await this.privacyRepository.All().AnyAsync(x => x.Id == privacy.Id);
            if (doesPrivacyExist)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.PrivacyAlreadyExists, privacy.Id));
            }

            await this.privacyRepository.AddAsync(privacy);
            await this.privacyRepository.SaveChangesAsync();

            var viewModel = await this.GetViewModelByIdAsync<PrivacyDetailsViewModel>(privacy.Id);

            return viewModel;
        }

        public async Task DeleteByIdAsync(int id)
        {
            var privacy = await this.privacyRepository.All().FirstOrDefaultAsync(x => x.Id == id);

            if (privacy == null)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.PrivacyNotFound, privacy.Id));
            }

            privacy.IsDeleted = true;
            privacy.DeletedOn = DateTime.UtcNow;
            this.privacyRepository.Update(privacy);
            await this.privacyRepository.SaveChangesAsync();
        }

        public async Task EditAsync(PrivacyEditViewModel privacyEditViewModel)
        {
            var privacy = await this.privacyRepository.All().FirstOrDefaultAsync(x => x.Id == privacyEditViewModel.Id);

            if (privacy == null)
            {
                throw new ArgumentException(
                    string.Format(ExceptionMessages.PrivacyNotFound, privacy.Id));
            }

            privacy.PageContent = privacyEditViewModel.PageContent;
            privacy.ModifiedOn = DateTime.UtcNow;

            this.privacyRepository.Update(privacy);
            await this.privacyRepository.SaveChangesAsync();
        }

        public async Task<TViewModel> GetViewModelByIdAsync<TViewModel>(int id)
        {
            var privacyViewModel = await this.privacyRepository
               .All()
               .Where(p => p.Id == id)
               .To<TViewModel>()
               .FirstOrDefaultAsync();

            return privacyViewModel;
        }

        public async Task<TViewModel> GetViewModelAsync<TViewModel>()
        {
            var privacyViewModel = await this.privacyRepository
               .All()
               .To<TViewModel>()
               .FirstOrDefaultAsync();

            return privacyViewModel;
        }
    }
}
