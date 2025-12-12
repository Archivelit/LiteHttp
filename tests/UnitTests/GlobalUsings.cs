global using System.Net;
global using System.Net.Sockets;
global using System.Text;
global using System.IO.Pipelines;

global using FluentAssertions;

global using LiteHttp.Constants;
global using LiteHttp.Enums;
global using LiteHttp.Logging;
global using LiteHttp.Models;
global using LiteHttp.Models.Events;
global using LiteHttp.RequestProcessors;
global using LiteHttp.Server;
global using LiteHttp.Server.Services.Endpoints;
global using LiteHttp.Constants.ErrorCodes;

global using Moq;