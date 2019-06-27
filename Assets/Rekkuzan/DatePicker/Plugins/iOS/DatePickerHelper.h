#import <Foundation/Foundation.h>

extern void UnitySendMessage(const char *, const char *, const char *);

@interface DatePickerHelper : NSObject<UIAlertViewDelegate>

@property (nonatomic) NSString *nameUnityObject;
@property (nonatomic) NSString *nameUnityMethodName;
@property (nonatomic) UIDatePicker *datePicker;

- (void) triggerDatePicker: (NSString *)nameObject  forMethodName:(NSString*)methodName;

@end
