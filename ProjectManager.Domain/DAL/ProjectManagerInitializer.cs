using ProjectManager.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProjectManager.DAL
{
    public class ProjectManagerInitializer : System.Data.Entity.CreateDatabaseIfNotExists<ProjectManagerContext>
    {
        protected override void Seed(ProjectManagerContext context)
        {
            var users = new List<User>
            {
                new User{Email="adrian@gmail.com", Password="12345678", Roles = "admin"},
                new User{Email="rudiadrian@o2.pl", Password="12345678", Roles="user", Name="Adrian", Surname="Rutkowski"}
            };

            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();

            var bug_types = new List<BugType>
            {
                new BugType{Name="Critical"}
            };

            bug_types.ForEach(b => context.BugTypes.Add(b));
            context.SaveChanges();

            var bug_statuses = new List<BugStatus>
            {
                new BugStatus{Name="New"},
                new BugStatus{Name="Reopen"},
                new BugStatus{Name="Assigned"},
                new BugStatus{Name="Resolved"}
            };

            bug_statuses.ForEach(b => context.BugStatuses.Add(b));
            context.SaveChanges();

            var task_priorities = new List<TaskPriority>
            {
                new TaskPriority{Name="Normal"},
                new TaskPriority{Name="High"}
            };

            task_priorities.ForEach(t => context.TaskPriorities.Add(t));
            context.SaveChanges();

            var task_statuses = new List<TaskStatus>
            {
                new TaskStatus{Name="New"},
                new TaskStatus{Name="Assigned"},
                new TaskStatus{Name="Finished"},
                new TaskStatus{Name="Reopen"}
            };

            task_statuses.ForEach(t => context.TaskStatuses.Add(t));
            context.SaveChanges();
        }
    }
}