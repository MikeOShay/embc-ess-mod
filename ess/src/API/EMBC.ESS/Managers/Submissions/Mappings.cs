﻿// -------------------------------------------------------------------------
//  Copyright © 2021 Province of British Columbia
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  https://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
// -------------------------------------------------------------------------

using System;
using AutoMapper;

namespace EMBC.ESS.Managers.Submissions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Shared.Contracts.Submissions.EvacuationFile, Resources.Cases.EvacuationFile>()
                .ForMember(d => d.NeedsAssessment, opts => opts.MapFrom(s => s.NeedsAssessment))
                .ForPath(d => d.NeedsAssessment.EvacuatedFrom, opts => opts.MapFrom(s => s.EvacuatedFromAddress))
                .ForMember(d => d.HouseholdMembers, opts => opts.Ignore())
                .ForMember(d => d.EvacuationDate, opts => opts.MapFrom(s => !s.EvacuationDate.HasValue ? DateTime.UtcNow : s.EvacuationDate))
                .ForMember(d => d.CreatedOn, opts => opts.Ignore())
                .ForMember(d => d.LastModified, opts => opts.Ignore())
                .ForMember(d => d.IsSecurityPhraseMasked, opts => opts.Ignore())
                .ForMember(d => d.TaskId, opts => opts.MapFrom(s => s.RelatedTask == null ? null : s.RelatedTask.Id))
                .ForMember(d => d.TaskLocationCommunityCode, opts => opts.Ignore())
                ;

            CreateMap<Resources.Cases.EvacuationFile, Shared.Contracts.Submissions.EvacuationFile>()
                .ForMember(d => d.EvacuatedFromAddress, opts => opts.MapFrom(s => s.EvacuatedFrom))
                .ForMember(d => d.RelatedTask, opts => opts.MapFrom(s => new Shared.Contracts.Submissions.IncidentTask { Id = s.TaskId }))
                ;

            CreateMap<Shared.Contracts.Submissions.Address, Resources.Cases.EvacuationAddress>()
                .ForMember(d => d.CommunityCode, opts => opts.MapFrom(s => s.Community))
                .ReverseMap()
                .ForMember(d => d.Community, opts => opts.MapFrom(s => s.CommunityCode))
                ;

            CreateMap<Shared.Contracts.Submissions.HouseholdMember, Resources.Cases.HouseholdMember>()
                .ForMember(d => d.HasAccessRestriction, opts => opts.Ignore())
                .ForMember(d => d.IsVerifiedRegistrant, opts => opts.Ignore())
                ;

            CreateMap<Resources.Cases.HouseholdMember, Shared.Contracts.Submissions.HouseholdMember>()
                .ForMember(d => d.RestrictedAccess, opts => opts.MapFrom(s => s.HasAccessRestriction))
                .ForMember(d => d.Verified, opts => opts.MapFrom(s => s.IsVerifiedRegistrant))
                ;

            CreateMap<Shared.Contracts.Submissions.Pet, Resources.Cases.Pet>()
                .ForMember(d => d.Id, opts => opts.Ignore())
                .ReverseMap()
                ;

            CreateMap<Shared.Contracts.Submissions.NeedsAssessment, Resources.Cases.NeedsAssessment>()
                .ForMember(d => d.EvacuatedFrom, opts => opts.Ignore())
                .ForMember(d => d.LastModified, opts => opts.Ignore())
                .ForMember(d => d.LastModifiedTeamMemberId, opts => opts.Ignore())
                .ForMember(d => d.CompletedByTeamMemberId, opts => opts.MapFrom(s => s.CompletedBy == null ? null : s.CompletedBy.Id))
                .ForMember(d => d.CompletedOn, opts => opts.MapFrom(s => s.CompletedOn == default ? DateTime.UtcNow : s.CompletedOn))
                ;

            CreateMap<Resources.Cases.NeedsAssessment, Shared.Contracts.Submissions.NeedsAssessment>()
                .ForMember(d => d.CompletedBy, opts => opts.MapFrom(s => s.CompletedByTeamMemberId == null ? null : new Shared.Contracts.Submissions.TeamMember
                {
                    Id = s.CompletedByTeamMemberId
                }))
                ;

            CreateMap<Shared.Contracts.Submissions.Note, Resources.Cases.Note>()
                .ReverseMap()
                .ForMember(d => d.MemberName, opts => opts.Ignore())
                .ForMember(d => d.TeamId, opts => opts.Ignore())
                .ForMember(d => d.TeamName, opts => opts.Ignore())
                ;

            CreateMap<Shared.Contracts.Submissions.RegistrantProfile, Resources.Contacts.Contact>()
                .ForMember(d => d.Authenticated, opts => opts.MapFrom(s => s.AuthenticatedUser))
                .ForMember(d => d.Verified, opts => opts.MapFrom(s => s.VerifiedUser))
                ;
            CreateMap<Resources.Contacts.Contact, Shared.Contracts.Submissions.RegistrantProfile>()
                .ForMember(d => d.AuthenticatedUser, opts => opts.MapFrom(s => s.Authenticated))
                .ForMember(d => d.VerifiedUser, opts => opts.MapFrom(s => s.Verified))
                ;

            CreateMap<Shared.Contracts.Submissions.Address, Resources.Contacts.Address>()
                .ReverseMap()
                ;

            CreateMap<Shared.Contracts.Submissions.SecurityQuestion, Resources.Contacts.SecurityQuestion>()
                .ForMember(d => d.AnswerIsMasked, opts => opts.MapFrom(s => !s.AnswerChanged))
                .ReverseMap()
                .ForMember(d => d.AnswerChanged, opts => opts.MapFrom(s => false))
                ;

            CreateMap<Resources.Tasks.EssTask, Shared.Contracts.Submissions.IncidentTask>();

            CreateMap<Resources.Cases.EvacuationFile, Shared.Contracts.Submissions.EvacuationFileSearchResult>()
                .ForMember(d => d.EvacuationAddress, opts => opts.MapFrom(s => s.EvacuatedFrom))
                ;

            CreateMap<Resources.Contacts.Contact, Shared.Contracts.Submissions.ProfileSearchResult>()
                .ForMember(d => d.RecentEvacuationFiles, opts => opts.Ignore())
                .ForMember(d => d.RegistrationDate, opts => opts.MapFrom(s => s.CreatedOn))
                .ForMember(d => d.IsVerified, opts => opts.MapFrom(s => s.Verified))
                ;

            CreateMap<Resources.Cases.HouseholdMember, Shared.Contracts.Submissions.EvacuationFileSearchResultHouseholdMember>()
                .ForMember(d => d.IsSearchMatch, opts => opts.Ignore())
                .ForMember(d => d.RestrictedAccess, opts => opts.MapFrom(s => s.HasAccessRestriction))
                ;
        }
    }
}
