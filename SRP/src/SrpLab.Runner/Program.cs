using SrpLab;

Console.WriteLine("SrpLab — 10 intentional SRP violations (refactor me)");
Console.WriteLine("=================================================");

var ward = new WardBoard();
ward.AssignBed(1, "p-88", heartRate: 130, spo2: 89);
var handoff = new HabdoffNote();
Console.WriteLine(handoff.BuildHandoffNote(ward, 1));
Console.WriteLine(string.Join(" | ", ward.DrainPagerLog()));

var basket = new CheckoutBasket();
basket.AddLine("SKU-1", 40m, 2);
basket.ApplyCouponText("SAVE10");
basket.EnableGiftWrap();
var basketTotalCalculator = new BasketTotalCalculator();
var paymentAuthorizer = new PaymentAuthorizerStub();
Console.WriteLine($"basket total={basketTotalCalculator.GrandTotal(basket)} auth={paymentAuthorizer.AuthorizePaymentStub("4242", basket, basketTotalCalculator)}");

var ticket = new SupportTicket("T-1", "cannot login", "prod is down for me", DateTimeOffset.UtcNow);
var slaCalculator = new SlaCalculator();
var publicReplyBuilder = new PublicReplyBuilder();
Console.WriteLine(publicReplyBuilder.DraftPublicReply(ticket, "Nora", slaCalculator));

var loan = new LoanDesk(60_000m, 640, 4, hasCollateral: false);
var riskCalculator = new RiskCalculator();
var eligibilityChecker = new EligibilityChecker();
var documentsBuilder = new RequiredDocumentsBuilder();
var decisionLetterBuilder = new DecisionLetterBuilder();
Console.WriteLine(decisionLetterBuilder.DecisionLetter("Omar", loan, riskCalculator, eligibilityChecker, documentsBuilder));

var course = new CourseEnrollmentDesk("SEF-101", capacity: 1, tuition: 3000m);
var welcome = new CourseWelcomePacketMarkdown();
Console.WriteLine(course.Register("a@mail.com"));
Console.WriteLine(course.Register("b@mail.com"));
Console.WriteLine(welcome.WelcomePacketMarkdown("b@mail.com", "Bea", course));

var kitchen = new KitchenTicket();
kitchen.AddItem("Pasta", new[] { "wheat", "milk" }, 12);
var renderer = new ThermalTicketRenderer();
Console.WriteLine(renderer.RenderThermalTicket(kitchen, 42));



var sub = new SubscriptionBilling("c-9", 99m, new DateOnly(2026, 9, 1), new DateOnly(2026, 10, 1));
var bP = new BillingProrate();
var nI = new BillingNextInvoiceNumber();
var dunning = new BillingDunningEmail();
sub.RegisterFailedPayment();
Console.WriteLine(dunning.DunningEmail("Sara", new DateOnly(2026, 9, 20), bP, sub, nI));


var stock = new StockAllocator();
var walking = new WalkingOrderPlanner();
var pick = new WarehousePickList();
var scripter = new PickerScriptBuilder();
pick.AddNeed("BOLT", "A", 3, 10, 7);
pick.AddNeed("NUT", "B", 1, 5, 5);
Console.WriteLine(scripter.PickerScript(walking, pick, stock));



var grades = new GradeBook();
grades.Record("s1", 92);
grades.Record("s1", 88);
var average = new CalAverage();
var letter = new CalcLetter();
var honorRoll = new CalHonorRollPolicy();
var transcript = new CalTranscriptPlain();
Console.WriteLine(transcript.TranscriptPlain("s1", "Ali", average, grades, letter, honorRoll));



var smsReminder = new AppointmentSmsReminder();
var appt = new AppointmentDesk(new TimeOnly(9, 0), new TimeOnly(17, 0), 30);
var slot = appt.FindNextSlot(DateTimeOffset.Parse("2026-09-21T08:00:00Z"), 48);
if (slot is null) throw new InvalidOperationException("no slot");
appt.TryBook(slot.Value);
Console.WriteLine(smsReminder.SmsReminder(slot.Value, "0100"));
Console.WriteLine("Done. Now split responsibilities — without breaking behavior.");
