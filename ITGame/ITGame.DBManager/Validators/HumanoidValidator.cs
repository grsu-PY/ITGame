using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using ITGame.DBManager.ViewModels;
using ITGame.Models.Creature;

namespace ITGame.DBManager.Validators
{
    public class HumanoidValidator : AbstractValidator<HumanoidViewModel>
    {
        public HumanoidValidator()
        {
            CascadeMode = CascadeMode.StopOnFirstFailure;
            this.RuleFor(humanoid => humanoid.Level)
                .NotEmpty()
                .InclusiveBetween((byte) 1, (byte) 100);
            this.RuleFor(humanoid => humanoid.Name)
                .NotEmpty()
                .WithMessage("Specify the name for the character, please");
            this.RuleFor(humanoid => humanoid.HumanoidRaceType)
                .NotEqual(HumanoidRaceType.None)
                .WithMessage("The Race should be selected.");
            this.RuleFor(humanoid => humanoid.FileName)
                .NotEmpty()
                .MustAsync(Predicate)
                .WithMessage("Path should point to existing file.");
        }

        private async Task<bool> Predicate(string path)
        {
            int i = 4;
            while (i>0)
            {
                await Task.Delay(250);
                i--;
            }
            return File.Exists(path);
        }
    }
}
