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
namespace Expenses.Model
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
        EmployeeMorale = 7210020,
        RecruitTravel = 722003,
        RecruitMeals = 722004,
        RecruitOther = 722005,
        Training = 725001,
        GeneralSupplies = 728002,
        ComputerSuppliesEquipment = 728004,
        ReferenceMaterial = 728007,
        ComputerServices = 740000,
        AdminServices = 740008,
        PhoneFax = 742000,
        CellPhone = 742001,
        HomeBroadband = 742002,
        TravelBroadband = 742016,
        Postage = 752000,
    }

    public enum ExpenseReportStatus
    {
        Saved = 0,
        Pending = 1,
        Approved = 2,
        Canceled = 3,
        Rejected = 4
    }

    public enum ExpenseItemType
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

    public enum SortType
    {
        Category = 0,
        Age = 1,
        Amount = 2,
        Submitter = 3,
        ItemType = 4
    }
}