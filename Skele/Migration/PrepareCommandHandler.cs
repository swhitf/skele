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

        public PrepareCommandHandler(ICommandContext context)
            : base(context)
        {
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
                !Presenter.Confirm(PREP_MSG))
            {
                //User cancelled.
                return CancelledResult();
            }

            var project = new Project();
            project.Name = Presenter.Prompt("Enter a project name:");

            if (Presenter.Confirm("Do you want to use the default project structure?"))
            {
                project.MigrationsPath = "Migrations";
                project.SnapshotsPath = "Snapshots";
            }
            else
            {
                project.MigrationsPath = Presenter.Prompt("Enter the migrations folder path:");
                project.SnapshotsPath = Presenter.Prompt("Enter the snapshots folder path:");
            }

            project.Targets.Add(new ProjectTarget(
                "default", Presenter.Prompt("Enter the default target connection string:")));

            new ProjectSerializer().Serialize(
                project, Path.Combine(dir.FullName, Project.DEFAULT_FILE));

            dir.CreateSubdirectory("Migrations");
            dir.CreateSubdirectory("Snapshots");

            return SuccessResult();
        }
    }
}
