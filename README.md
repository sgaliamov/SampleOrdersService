# Orders service

The solution contains implementation of a typical technical assignment that a company could give to have topics for discussion on an interview.

## Implementation details

This section contains description and reasoning about technical decisions.

### Top down development

Implementation was started from UI application with fake backend. Such approach helps faster demonstrate to a client what he will get at the end and receive very important feedback on early stages of development. It also helps define interfaces and data contracts between UI application and API.

### CQRS

The project have simple implementation of Command and Query Responsibility Segregation pattern.

`OrdersQueryController` and `OrdersPresenter` represents the Query part. Their responsibility is to retrieve data from DB and convert it to DTO. `OrdersCommandController`, `ICommandBuilder` and `ICommandDispatcher` are responsible for commands processing.

In this particular case using `CQRS` pattern is over-engineering. But in a big high-load project I would seriously consider this approach to able horizontally scale an application.

### DIP and SOLID

The design of application is created having SOLID principles in mind. For example, `IOrdersRepository` is defined in `OrdersService.BusinessLogic.Contracts`. Thus Data access layer have to follow business logic, but not vise versa (DIP).

### Angular

Why Angular? It's enterprise level framework which is implemented following latest and future industrial standards such as [Web Components](https://en.wikipedia.org/wiki/Web_Components) and [WebAssembly](https://en.wikipedia.org/wiki/WebAssembly). [Blazor](https://blazor.net/) could be good candidate for .net developers.

### Redux approach with NgRx

For UI big projects managing state is challenging.  [NgRx](https://github.com/ngrx/platform) makes development easy and fun. `Ngrx` are [mature](https://gist.github.com/btroncone/a6e4347326749f938510) enough to use it in production.

## Descoped features

The goal of the project to have topics for the discussion on an interview and to not implement full blown solution for a specific domain. That way some features are not implemented now, but should be applied for a real situation. This list could be used as a check list for a real project.

**Flex layout** - ability to adapt UI for any screens. There are good library [Angular Flex-Layout](https://github.com/angular/flex-layout) which is compatible with [Angular Material](https://material.angular.io/).

**Authentication** - `OAuth2` is good candidate to implement authentication. And of course, we should not forget about authorization logic.

**Docker containers** - it's pretty easy to run UI and backend in a containers. Especially, if we use `.NET Core`.

**[Route guards](https://angular.io/guide/router#guards)** - Angular feature that can warn a user that before doing important or irreparable actions.

**Configuration** - sounds obvious, but I saw big projects with hard coded magical constants and it was sad.

**[oData](https://www.odata.org/)** - good candidate how to define querying interface. With [EF](https://docs.microsoft.com/en-us/aspnet/web-api/overview/odata-support-in-aspnet-web-api/odata-v4/create-an-odata-v4-endpoint) it could speed up development on early stated.

**Optimistic locks** - it's a good practice to prevent users concurrently override data. We should provide versioning data for each update.

**DDD and aggregates** - for big complex domain we could apply [DDD](https://en.wikipedia.org/wiki/Domain-driven_design) practices and create domain aggregates. Personally, I prefer anemic model and functional programming style.

**Query handler to improve CQRS** - in complex cases we could have a query handler, similar to `ICommandDispatcher` for the commands flow.

**Message queues** - commands and queries don't have to be processed in one backend process. Using [RPC](https://www.rabbitmq.com/tutorials/tutorial-six-dotnet.html) we can use messages even to request data.

**Separate services** - api should be simple as possible, all business logic should be in a separate background services which communicate with messages.

**Event sourcing** - having commands log we able to restore state of a system for any moment.

**Separate repositories to process commands** - now the project have one repository for everything. But it could be good decision to separate repositories for altering and querying data. It will help to use proper tool in each case and focus on performance where it necessary.

**Dapper** - very good micro ORM, good candidate to implement commands processing.

**Global exception handler** - no exception should stay untracked. I like log into `Kibana` using `Serilog`.

**Wrapping 3rd party components** - for long term projects it's important to stay flexible and to not depend on 3rd party. Sometimes they die or better option is coming.

**E2e test** - another good reason use Angular is [Protractor](https://www.protractortest.org/#/). Extremely useful testing tool.

Of course, the list is not complete and will be extended.

## Swagger Best Practices

- Define type for `ActionResult`.
- Use `Microsoft.AspNetCore.Mvc.Api.Analyzers`. But it will not cover all cases.
- Define only that global `ProducesResponseType` attributes which are processed globally automatically.
- Define rest `ProducesResponseType` attributes when an action returns multiple status codes.
- Or use `DefaultApiConventions` for simple generic APIs.
- Add XML docs.
- Define supported media types explicitly (`ProducesAttribute`, `ConsumesAttribute`).
- Use `Microsoft.AspNetCore.Mvc.Api.Versioning` to supprot versioning.
