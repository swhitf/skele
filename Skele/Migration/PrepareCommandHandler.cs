using Skele.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Skele.Migration
{
    class PrepareCommandHandler : CommandHandlerBase<PrepareCommand>
    {
        private const string PREP_MSG = "The specified directory is not empty, are you sure you want to prepare your project here?";
        
        private IPresenter presenter;

        public PrepareCommandHandler(ICommandContext context, IPresenter presenter)
            : base(context)
        {
            this.presenter = presenter;
        }

        public override int Execute(PrepareCommand input)
        {
            var dir = new DirectoryInfo(input.DirectoryPath);

            if (!dir.Exists)
            {
                dir.Create();
            }

            if (dir.GetFiles(Project.DEFAULT_FILE).Any())
            {
                return FailureResult("Cannot create project here; another project already exists.");
            }

            if ((dir.EnumerateFiles().Any() || dir.EnumerateDirectories().Any()) &&
                !presenter.Confirm(PREP_MSG))
            {
                //User cancelled.
                return CancelledResult();
            }

            var project = new Project();
            project.Name = presenter.Prompt("Enter a project name:");

            if (presenter.Confirm("Do you want to use the default project structure?"))
            {
                project.MigrationsPath = "Migrations";
                project.SnapshotsPath = "Snapshots";
            }
            else
            {
                project.MigrationsPath = presenter.Prompt("Enter the migrations folder path:");
                project.SnapshotsPath = presenter.Prompt("Enter the snapshots folder path:");
            }

            project.Targets.Add(new ProjectTarget(
                "default", presenter.Prompt("Enter the default target connection string:")));

            new ProjectSerializer().Serialize(
                project, Path.Combine(dir.FullName, Project.DEFAULT_FILE));

            dir.CreateSubdirectory("Migrations");
            dir.CreateSubdirectory("Snapshots");

            return SuccessResult();
        }
    }
}
