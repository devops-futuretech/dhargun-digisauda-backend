//Its used for common settings
var emamiGlobal = emamiGlobal || {};
emamiGlobal.wizard = emamiGlobal.wizard || {};

//To set wizard section
emamiGlobal.wizard.section = emamiGlobal.wizard.section || {
    Location: 0
};

var currentDate = new Date();
//To set constant value
emamiGlobal.constant = emamiGlobal.constant || {
    PageSize: 10,
    DateFormat: "dd-MMM-yyyy",
    DateTimeFormat: "dd-MMM-yyyy hh:mm tt",
    TimeFormat: "hh:mm tt",
    EnterKeyCode: 13,
    mobileNoValidadtionValue: "9876",
    pincodeString: "27,31",
    MaximumBulletinImages: 20,
    MaximumBulletinVideoUrls: 3,
    DefaultImageUrl: "/images/default.jpg",
    AcceptedImageFormat: [".jpg", ".jpeg", ".png", ".pdf", ".mp4"],
	RupeeSymbol: "₹",
    PercentageSymbol: "%",
    SearchDaysCount: 7,
    DefaultStateName: 'Tamil Nadu',
    DropdownAllId: -1,
    DefaultDecimalPoints: 2,
    ReportDateRange: 30,
    TimeFormat24: "HH:mm tt",
    TimeFormat12: "hh:mm tt",
    TimeInterval: 4,
    SetTimeOut: 5000,
    IndentDateRange: 90,
    DefaultEndDate: new Date(currentDate.getFullYear(), currentDate.getMonth(), currentDate.getDate()),
    BiddingWindowDashboardFormat: '{0:0}'
};

//To set model type
emamiGlobal.country = emamiGlobal.country || {
    India: 1,
    Other: 0
};


//To set model type
emamiGlobal.errorMessage = emamiGlobal.errorMessage || {
    ProfileUploadOneFile: "Please select only 1 file.",
    AcceptedImageFormat: "File format must be jpeg, png, jpg, pdf or mp4",
    EnterValidEmail: "Please enter a valid email",
    EnterValidMobileNumber: "Please enter a valid mobile number",
};

//To set order status
emamiGlobal.Status = emamiGlobal.Status || {
    Pending: 1,
    Approved: 2,
    Rejected: 3,
    Revised: 4,
    Hold: 5,
    Completed: 6,
    WaitingForApproval: 7,
    Processed: 8,
    RequestForApproval: 9,
    RequestForApproval2: 10
};

//To set the verticals
emamiGlobal.Verticals = emamiGlobal.Verticals || {
    HBC: 1,
    SpecialityFat: 2,
    Rasoi: 3   
};

//To set the LooseVertical
emamiGlobal.LooseVertical = emamiGlobal.LooseVertical || {
    HBC: 1,
    SpecialityFat: 2,
    Loose: 3
};

//To set the verticals
emamiGlobal.SaudaBookingType = emamiGlobal.SaudaBookingType || {
    TraditionalProcess: 1,
    ReverseAuction: 2
};

emamiGlobal.PublishButtonStatus = emamiGlobal.PublishButtonStatus || {
    PriceGenerating: 1,
    Publish: 2,
    Published: 3,
    PriceGenerateFailed: 4
};


