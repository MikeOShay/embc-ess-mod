import { Injectable } from '@angular/core';
import { HouseholdMemberType } from 'src/app/core/api/models';
import { RegistrantProfileModel } from 'src/app/core/models/registrant-profile.model';
import { WizardType } from 'src/app/core/models/wizard-type.model';
import { EvacueeProfileService } from 'src/app/core/services/evacuee-profile.service';
import { EvacueeSessionService } from 'src/app/core/services/evacuee-session.service';
import { EvacueeSearchService } from '../search/evacuee-search/evacuee-search.service';
import { StepEssFileService } from './step-ess-file/step-ess-file.service';
import { StepEvacueeProfileService } from './step-evacuee-profile/step-evacuee-profile.service';
import { WizardService } from './wizard.service';

@Injectable({
  providedIn: 'root'
})
export class WizardAdapterService {
  constructor(
    private wizardService: WizardService,
    private evacueeSearchService: EvacueeSearchService,
    private evacueeSessionService: EvacueeSessionService,
    private evacueeProfileService: EvacueeProfileService,
    private stepEvacueeProfileService: StepEvacueeProfileService,
    private stepEssFileService: StepEssFileService
  ) {}

  /**
   * Clear all steps for current wizard type, usually before exiting wizard
   */
  public clearWizard(): void {
    const wizType = this.evacueeSessionService.getWizardType();

    switch (wizType) {
      case WizardType.NewRegistration:
        this.stepEvacueeProfileService.clearService();
        this.stepEssFileService.clearService();
        // Clear supports & notes
        return;

      case WizardType.EditRegistration:
        this.stepEvacueeProfileService.clearService();
        this.stepEssFileService.clearService();
        return;

      case WizardType.NewEssFile:
        this.stepEssFileService.clearService();
        return;

      case WizardType.MemberRegistration:
        // Clear steps
        return;

      case WizardType.ReviewFile:
        // Clear steps
        return;

      case WizardType.CompleteFile:
        // Clear steps
        return;
    }
  }

  /**
   * Set initial values for Create Registrant Profile (stepEvacueeProfileService) when entering from Evacuee Search
   */
  public createProfileFromSearch() {
    this.stepEvacueeProfileService.personalDetails = {
      ...this.stepEvacueeProfileService.personalDetails,
      firstName: this.evacueeSearchService.evacueeSearchContext
        ?.evacueeSearchParameters?.firstName,
      lastName: this.evacueeSearchService.evacueeSearchContext
        ?.evacueeSearchParameters?.lastName,
      dateOfBirth: this.evacueeSearchService.evacueeSearchContext
        ?.evacueeSearchParameters?.dateOfBirth
    };
  }

  /**
   * Set initial values for stepProfileService for editing the Profile when entering from Evacuee Profile Dashboard
   */
  public editProfileFromDashboard(profileId: string) {
    this.evacueeProfileService
      .getProfileFromId(profileId)
      .subscribe((registrantProfileModel) => {
        this.stepEvacueeProfileService.setFormValuesFromProfile(
          registrantProfileModel
        );
        this.stepEvacueeProfileService.setEditTabStatus();
      });
  }

  /**
   * Set initial values for Create ESS File (stepEssFileService) after creating/updating profile on wizard
   */
  public createEssFileFromProfileStep(profile: RegistrantProfileModel) {
    this.evacueeSessionService.profileId = profile.id;
    this.stepEssFileService.primaryAddress = this.wizardService.setAddressObjectForForm(
      profile.primaryAddress
    );

    this.stepEssFileService.primaryMember = {
      dateOfBirth: profile.personalDetails.dateOfBirth,
      firstName: profile.personalDetails.firstName,
      lastName: profile.personalDetails.lastName,
      gender: profile.personalDetails.gender,
      initials: profile.personalDetails.initials,
      sameLastName: true,
      isPrimaryRegistrant: true,
      type: HouseholdMemberType.Registrant
    };
  }

  /**
   * Set initial values for Create ESS File (stepEssFileService) when entering from Evacuee Profile Dashboard
   */
  public createEssFileFromDashboard() {
    //TODO
  }
}
