// ----------------------------------------------------------------------------------
// Microsoft Developer & Platform Evangelism
// 
// Copyright (c) Microsoft Corporation. All rights reserved.
// 
// THIS CODE AND INFORMATION ARE PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
// EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES 
// OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
// ----------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
// ----------------------------------------------------------------------------------

//*********************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
// IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
// PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.
//
//*********************************************************
namespace Expenses.WcfService.ServiceCore
{
    public enum CategoryType
    {
        AirFare = 0,
        CarRental = 1,
        ConferenceSeminar = 2,
        Entertainment = 3,
        Gifts = 4,
        Hotel = 5,
        Mileage = 6,
        Other = 7,
        OtherTravelAndLodging = 8,
        TEMeals = 9
    }

    public enum OtherCategoryType
    {
        EmployeeMorale = 0,
        RecruitTravel = 1,
        RecruitMeals = 2,
        RecruitOther = 3,
        Training = 4,
        GeneralSupplies = 5,
        ComputerSuppliesEquipment = 6,
        ReferenceMaterial = 7,
        ComputerServices = 8,
        AdminServices = 9,
        PhoneFax = 10,
        CellPhone = 11,
        HomeBroadband = 12,
        TravelBroadband = 13,
        Postage = 14
    }

    public enum ExpenseReportStatus
    {
        Saved = 0,
        Pending = 1,
        Approved = 2,
        Canceled = 3,
        Rejected = 4
    }

    public enum ChargeType
    {
        Business = 1,
        Personal = 2
    }

    public enum ApprovalActionType
    {
        Approve = 0,
        Reject = 1
    }

    public enum ItemType
    {
        Charge = 0,
        SavedReport = 1,
        PendingReport = 2,
        UnresolvedReport = 3,
        ApprovedReport = 4
    }
}