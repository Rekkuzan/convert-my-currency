#import "DatePickerHelper.h"

@implementation DatePickerHelper

-(void) triggerDatePicker: (NSString *)nameObject forMethodName:(NSString*)methodName
{
    self.nameUnityObject = nameObject;
    self.nameUnityMethodName = methodName;
    
    if (self.datePicker!=nil) {
        [self removeDatePicker:nil];
    }
    
    UIViewController *vc =  UnityGetGLViewController();
    CGRect toolbarTargetFrame = CGRectMake(0, vc.view.bounds.size.height-216-44, [self GetW], 44);
    CGRect datePickerTargetFrame = CGRectMake(0, vc.view.bounds.size.height-216, [self GetW], 216);
    CGRect darkViewTargetFrame = CGRectMake(0, vc.view.bounds.size.height-216-44, [self GetW], 260);
    UIView *darkView = [[UIView alloc] initWithFrame:CGRectMake(0, vc.view.bounds.size.height, [self GetW], 260)];
    darkView.alpha = 1;
    darkView.backgroundColor = [UIColor whiteColor];
    darkView.tag = 9;
    [vc.view addSubview:darkView];
    self.datePicker = [[UIDatePicker alloc] initWithFrame:CGRectMake(0, vc.view.bounds.size.height - 44, [self GetW], 216)];
    self.datePicker.tag = 10;
    [self.datePicker addTarget:self action:@selector(onDatePickerValueChanged:) forControlEvents:UIControlEventValueChanged];
    self.datePicker.datePickerMode = UIDatePickerModeDate;
    [vc.view addSubview:self.datePicker];
    UIToolbar *toolBar = [[UIToolbar alloc] initWithFrame:CGRectMake(0, vc.view.bounds.size.height, [self GetW], 44)];
    toolBar.tag = 11;
    toolBar.barStyle = UIBarStyleDefault;
    UIBarButtonItem *spacer = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemFlexibleSpace target:nil action:nil];
    UIBarButtonItem *doneButton = [[UIBarButtonItem alloc] initWithBarButtonSystemItem:UIBarButtonSystemItemDone target:self action:@selector(onDatePickerDismiss:)];
    [toolBar setItems:[NSArray arrayWithObjects:spacer, doneButton, nil]];
    [vc.view addSubview:toolBar];
    [UIView beginAnimations:@"MoveIn" context:nil];
    toolBar.frame = toolbarTargetFrame;
    self.datePicker.frame = datePickerTargetFrame;
    darkView.frame = darkViewTargetFrame;
    [UIView commitAnimations];
}

-(void) onDatePickerValueChanged:(UIDatePicker *)sender {
    
    NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
    [dateFormatter setDateFormat: @"yyyy-MM-dd"];
    NSString *dateString = [dateFormatter stringFromDate:sender.date];
    UnitySendMessage(self.nameUnityObject.UTF8String, self.nameUnityMethodName.UTF8String, dateString.UTF8String);
}

- (void) onDatePickerDismiss:(id)sender
{
    UIViewController *vc =  UnityGetGLViewController();
    [self OnDatePickerClosed:self.datePicker];
    CGRect toolbarTargetFrame = CGRectMake(0, vc.view.bounds.size.height, [self GetW], 44);
    CGRect datePickerTargetFrame = CGRectMake(0, vc.view.bounds.size.height - 44, [self GetW], 216);
    CGRect darkViewTargetFrame = CGRectMake(0, vc.view.bounds.size.height, [self GetW], 260);
    [UIView beginAnimations:@"MoveOut" context:nil];
    [vc.view viewWithTag:9].frame = darkViewTargetFrame;
    [vc.view viewWithTag:10].frame = datePickerTargetFrame;
    [vc.view viewWithTag:11].frame = toolbarTargetFrame;
    [UIView setAnimationDelegate:self];
    [UIView setAnimationDidStopSelector:@selector(OnDatePickerClosed)];
    [UIView commitAnimations];
}

-(void) OnDatePickerClosed:(UIDatePicker *)sender
{
    NSDateFormatter *dateFormatter = [[NSDateFormatter alloc] init];
    [dateFormatter setDateFormat: @"yyyy-MM-dd"];
    NSString *dateString = [dateFormatter stringFromDate:sender.date];
    UnitySendMessage(self.nameUnityObject.UTF8String, self.nameUnityMethodName.UTF8String, dateString.UTF8String);
}

- (void)removeDatePicker:(id)object
{
    if(self.datePicker==nil)
    {
        return;
    }
    UIViewController *vc =  UnityGetGLViewController();
    [[vc.view viewWithTag:9] removeFromSuperview];
    [[vc.view viewWithTag:10] removeFromSuperview];
    [[vc.view viewWithTag:11] removeFromSuperview];
}

-(CGFloat) GetW
{
    UIViewController *vc = UnityGetGLViewController();
    bool IsLandscape;
    UIInterfaceOrientation orientation = [UIApplication sharedApplication].statusBarOrientation;
    if(orientation == UIInterfaceOrientationLandscapeLeft ||
       orientation == UIInterfaceOrientationLandscapeRight)
    {
        IsLandscape = true;
    }
    else
    {
        IsLandscape = false;
    }
    CGFloat w;
    if(IsLandscape)
    {
        w = vc.view.frame.size.height;
    }
    else
    {
        w = vc.view.frame.size.width;
    }
    
    NSArray *vComp = [[UIDevice currentDevice].systemVersion componentsSeparatedByString:@"."];
    if ([[vComp objectAtIndex:0] intValue] >= 8)
    {
        w = vc.view.frame.size.width;
    }
    return w;
}

@end

extern "C" {
    DatePickerHelper *sInstance;
    
    void StartDatePicker(char * nameObject, char * methodName)
    {
        if(sInstance == nil) {
            // Create our instance
            sInstance = [DatePickerHelper alloc];
        }
        
        [sInstance triggerDatePicker:[NSString stringWithUTF8String:nameObject] forMethodName:[NSString stringWithUTF8String:methodName]];
    }
}
