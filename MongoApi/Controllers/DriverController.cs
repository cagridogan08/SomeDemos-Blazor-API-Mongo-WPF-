﻿using Microsoft.AspNetCore.Mvc;
using MongoApi.Models;
using MongoApi.Services;

namespace MongoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DriverController : ControllerBase
{
    private readonly DriverService _driverService;
    public DriverController(DriverService driverService)
    {
        _driverService = driverService;
    }
    [HttpGet]
    public async Task<ICollection<Driver>> Get() => await _driverService.GetDrivers();

    [HttpPost]
    public async Task<bool> Create(Driver driver) => await _driverService.CreateDriver(driver);

    [HttpDelete("{id}")]
    public async Task<bool> Delete(string id) => await _driverService.DeleteDriver(id);
    [HttpPut]
    public async Task<bool> Update(Driver driver) => await _driverService.UpdateDriver(driver);

}