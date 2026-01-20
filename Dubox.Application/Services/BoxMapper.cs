using Dubox.Application.DTOs;
using Dubox.Domain.Abstraction;
using Dubox.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dubox.Application.Services
{
    public interface IBoxMapper
    {
        BoxDto Map(Box box);
    }
    public class BoxMapper : IBoxMapper
    {
        private readonly IUnitOfWork _unitOfWork;
        public BoxMapper( IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public BoxDto Map(Box box)
        {
            try
            {
                var projectCode = box.Project?.ProjectCode ?? string.Empty;
                var client = box.Project?.ClientName ?? string.Empty;

                var boxTypeId = box.ProjectBoxTypeId;
                var boxSubTypeId = box.ProjectBoxSubTypeId;
                string boxType = string.Empty;
                string? boxSubTypeName = null;

                // Fetch BoxType name from ProjectBoxTypes
                if (boxTypeId.HasValue)
                {
                    var projectBoxType = _unitOfWork.Repository<ProjectBoxType>()
                        .Get()
                        .FirstOrDefault(pbt => pbt.Id == boxTypeId.Value && pbt.ProjectId == box.ProjectId);
                    boxType = projectBoxType?.TypeName ?? string.Empty;
                }

                // Fetch BoxSubType name from ProjectBoxSubTypes
                if (boxSubTypeId.HasValue)
                {
                    var projectBoxSubType = _unitOfWork.Repository<ProjectBoxSubType>()
                        .Get()
                        .FirstOrDefault(pbst => pbst.Id == boxSubTypeId.Value);
                    boxSubTypeName = projectBoxSubType?.SubTypeName;
                }

                // Get Zone - stored as ZoneCode string in database
                string? zoneString = null;
                if (!string.IsNullOrEmpty(box.Zone))
                {
                    zoneString = box.Zone;
                }

                // Safely get Status (enum conversion)
                var statusString = box.Status.ToString();

                // Safely get UnitOfMeasure (enum conversion)
                string? unitOfMeasureString = null;
                if (box.UnitOfMeasure.HasValue)
                {
                    unitOfMeasureString = box.UnitOfMeasure.Value.ToString();
                }

                // Safely get CurrentLocation information
                var currentLocationId = box.CurrentLocationId;
                var currentLocationCode = box.CurrentLocation?.LocationCode;
                var currentLocationName = box.CurrentLocation?.LocationName;

                // Safely get Factory information
                var factoryId = box.FactoryId;
                var factoryCode = box.Factory?.FactoryCode;
                var factoryName = box.Factory?.FactoryName;

                // Get ActivitiesCount
                var activitiesCount = box.BoxActivities?.Count ?? 0;

                var boxPanels = box.BoxPanels?.Select(p => new BoxPanelDto
                {
                    BoxPanelId = p.BoxPanelId,
                    BoxId = p.BoxId,
                    ProjectId = p.ProjectId,
                    PanelName = p.PanelName,
                    PanelStatus = p.PanelStatus,
                    CreatedDate = p.CreatedDate,
                    ModifiedDate = p.ModifiedDate
                }).ToList() ?? new List<BoxPanelDto>();

                return new BoxDto
                {
                    BoxId = box.BoxId,
                    ProjectId = box.ProjectId,
                    ProjectCode = projectCode,
                    ProjectStatus = box.Project?.Status.ToString(),
                    Client = client,
                    BoxTag = box.BoxTag ?? string.Empty,
                    SerialNumber = box.SerialNumber,
                    BoxName = box.BoxName,
                    BoxType = boxType,
                    BoxTypeId = boxTypeId,
                    BoxSubTypeId = boxSubTypeId,
                    BoxSubTypeName = boxSubTypeName,
                    Floor = box.Floor,
                    BuildingNumber = box.BuildingNumber,
                    BoxFunction = box.BoxFunction,
                    Zone = zoneString,
                    ProgressPercentage = box.ProgressPercentage,
                    Status = statusString,
                    Length = box.Length,
                    Width = box.Width,
                    Height = box.Height,
                    UnitOfMeasure = unitOfMeasureString,
                    RevitElementId = box.RevitElementId,
                    Duration = box.Duration,
                    PlannedStartDate = box.PlannedStartDate,
                    ActualStartDate = box.ActualStartDate,
                    PlannedEndDate = box.PlannedEndDate,
                    ActualEndDate = box.ActualEndDate,
                    CreatedDate = box.CreatedDate,
                    ActivitiesCount = activitiesCount,
                    Notes = box.Notes,
                    CurrentLocationId = currentLocationId,
                    CurrentLocationCode = currentLocationCode,
                    CurrentLocationName = currentLocationName,
                    FactoryId = factoryId,
                    FactoryCode = factoryCode,
                    FactoryName = factoryName,
                    Bay = box.Bay,
                    Row = box.Row,
                    Position = box.Position,
                    BoxPanels = boxPanels,
                    Slab = box.Slab,
                    Soffit = box.Soffit,
                    PodDeliver = box.PodDeliver,
                    PodName = box.PodName,
                    PodType = box.PodType, 
                    BoxNumber=box.BoxNumber
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error mapping Box {box.BoxId} to BoxDto. Property causing error: {ex.Message}", ex);
            }
        }
    }
    }

