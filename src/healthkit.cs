//
// This file describes the API that the generator will produce
//
// Authors:
//   Alex Soto alex.soto@xamarin.com
//   Miguel de Icaza (miguel@xamarin.com)
//   Whitney Schmidt (whschm@microsoft.com)
//
// Copyright 2014-2015 Xamarin Inc.
// Copyright 2019, 2020 Microsoft Corporation. All rights reserved.
//

using CoreFoundation;
using ObjCRuntime;
using Foundation;
using System;
using System.ComponentModel;
using CoreLocation;

#if !NET
using NativeHandle = System.IntPtr;
#endif

namespace HealthKit {

	[Watch (3,0), iOS (10,0)]
	public enum HKDocumentTypeIdentifier {
		[Field ("HKDocumentTypeIdentifierCDA")]
		Cda,
	}

	// NSInteger -> HKDefines.h
	[Watch (2,0)]
	[iOS (8,0)]
	[ErrorDomain ("HKErrorDomain")]
	[Native]
	public enum HKErrorCode : long {
		NoError = 0,
		HealthDataUnavailable,
		HealthDataRestricted,
		InvalidArgument,
		AuthorizationDenied,
		AuthorizationNotDetermined,
		DatabaseInaccessible,
		UserCanceled,
		AnotherWorkoutSessionStarted,
		UserExitedWorkoutSession,
		RequiredAuthorizationDenied,
		NoData,
	}

	[iOS (10,0)]
	[Watch (2,0)]
	[Native]
	public enum HKWorkoutSessionLocationType : long {
		Unknown = 1,
		Indoor,
		Outdoor
	}

	[NoiOS]
	[Watch (2,0)]
	[Native]
	public enum HKWorkoutSessionState : long {
		NotStarted = 1,
		Running,
		Ended,
		[Watch (3,0)]
		Paused,
		[Watch (5,0)]
		Prepared,
		[Watch (5,0)]
		Stopped,
	}

	[iOS (11,0)]
	[Watch (4,0)]
	[Native]
	public enum HKHeartRateMotionContext : long {
		NotSet = 0,
		Sedentary,
		Active,
	}

	[Watch (7,0), iOS (14,0)]
	[Native]
	public enum HKActivityMoveMode : long {
		ActiveEnergy = 1,
		AppleMoveTime = 2,
	}

	[Watch (7,1), iOS (14,2)]
	[Native]
	public enum HKCategoryValueHeadphoneAudioExposureEvent : long {
		SevenDayLimit = 1,
	}

	[Watch (8,0), iOS (15,0)]
	[Native]
	public enum HKAppleWalkingSteadinessClassification : long {
		Ok = 1,
		Low,
		VeryLow,
	}

	[Watch (8,0), iOS (15,0)]
	[Native]
	public enum HKCategoryValueAppleWalkingSteadinessEvent : long {
		InitialLow = 1,
		InitialVeryLow = 2,
		RepeatLow = 3,
		RepeatVeryLow = 4,
	}

	[Watch (8,0), iOS (15,0)]
	[Native]
	public enum HKCategoryValuePregnancyTestResult : long {
		Negative = 1,
		Positive,
		Indeterminate,
	}

	[Watch (8,0), iOS (15,0)]
	[Native]
	public enum HKCategoryValueProgesteroneTestResult : long {
		Negative = 1,
		Positive,
		Indeterminate,
	}

#if NET
	delegate void HKAnchoredObjectResultHandler (HKAnchoredObjectQuery query, HKSample[] results, nuint newAnchor, NSError error);
#else
	delegate void HKAnchoredObjectResultHandler2 (HKAnchoredObjectQuery query, HKSample[] results, nuint newAnchor, NSError error);

	[Obsolete ("Use HKAnchoredObjectResultHandler2 instead")]
	delegate void HKAnchoredObjectResultHandler (HKAnchoredObjectQuery query, HKSampleType[] results, nuint newAnchor, NSError error);
#endif

	delegate void HKAnchoredObjectUpdateHandler (HKAnchoredObjectQuery query, HKSample[] addedObjects, HKDeletedObject[] deletedObjects, HKQueryAnchor newAnchor, NSError error);

	delegate void HKWorkoutRouteBuilderDataHandler (HKWorkoutRouteQuery query, CLLocation [] routeData, bool done, NSError error);

	[Watch (2,0)]
	[iOS (8,0)]
	[BaseType (typeof (HKQuery))]
	[DisableDefaultCtor] // NSInvalidArgumentException: The -init method is not available on HKAnchoredObjectQuery
	interface HKAnchoredObjectQuery {

		[NoWatch]
#if !NET
		[Obsolete ("Use the overload that takes HKAnchoredObjectResultHandler2 instead")]
#endif
		[Deprecated (PlatformName.iOS, 9, 0)]
		[Export ("initWithType:predicate:anchor:limit:completionHandler:")]
		NativeHandle Constructor (HKSampleType type, [NullAllowed] NSPredicate predicate, nuint anchor, nuint limit, HKAnchoredObjectResultHandler completion);

#if !NET
		[NoWatch]
		[Sealed]
		[Deprecated (PlatformName.iOS, 9, 0)]
		[Export ("initWithType:predicate:anchor:limit:completionHandler:")]
		NativeHandle Constructor (HKSampleType type, [NullAllowed] NSPredicate predicate, nuint anchor, nuint limit, HKAnchoredObjectResultHandler2 completion);
#endif

		[iOS (9,0)]
		[Export ("initWithType:predicate:anchor:limit:resultsHandler:")]
		NativeHandle Constructor (HKSampleType type, [NullAllowed] NSPredicate predicate, [NullAllowed] HKQueryAnchor anchor, nuint limit, HKAnchoredObjectUpdateHandler handler);

		[Watch (8,0), iOS (15,0)]
		[Export ("initWithQueryDescriptors:anchor:limit:resultsHandler:")]
		NativeHandle Constructor (HKQueryDescriptor[] queryDescriptors, [NullAllowed] HKQueryAnchor anchor, nint limit, HKAnchoredObjectUpdateHandler resultsHandler);

		[iOS (9,0)]
		[NullAllowed, Export ("updateHandler", ArgumentSemantic.Copy)]
		HKAnchoredObjectUpdateHandler UpdateHandler { get; set; }
	}

	[Watch (2,0)]
	[iOS (8,0)]
	[Static]
	interface HKPredicateKeyPath {
		[Field ("HKPredicateKeyPathCategoryValue")]
		NSString CategoryValue { get; }

		[Field ("HKPredicateKeyPathSource")]
		NSString Source { get; }

		[Field ("HKPredicateKeyPathMetadata")]
		NSString Metadata { get; }
		
		[Field ("HKPredicateKeyPathQuantity")]
		NSString Quantity { get; }

		[Field ("HKPredicateKeyPathStartDate")]
		NSString StartDate { get; }

		[Field ("HKPredicateKeyPathEndDate")]
		NSString EndDate { get; }

		[Field ("HKPredicateKeyPathUUID")]
		NSString Uuid { get; }

		[Field ("HKPredicateKeyPathCorrelation")]
		NSString Correlation { get; }

		[Field ("HKPredicateKeyPathWorkout")]
		NSString Workout { get; }

		[Field ("HKPredicateKeyPathWorkoutDuration")]
		NSString WorkoutDuration { get; }
		
		[Field ("HKPredicateKeyPathWorkoutTotalDistance")]
		NSString WorkoutTotalDistance { get; }
		
		[Field ("HKPredicateKeyPathWorkoutTotalEnergyBurned")]
		NSString WorkoutTotalEnergyBurned { get; }
		
		[Field ("HKPredicateKeyPathWorkoutType")]
		NSString WorkoutType { get; }

		[Watch (3,0), iOS (10,0)]
		[Field ("HKPredicateKeyPathWorkoutTotalSwimmingStrokeCount")]
		NSString WorkoutTotalSwimmingStrokeCount { get; }

		[iOS (9,0)]
		[Field ("HKPredicateKeyPathDevice")]
		NSString Device { get; }

		[iOS (9,0)]
		[Field ("HKPredicateKeyPathSourceRevision")]
		NSString SourceRevision { get; }

		[iOS (9,3), Watch (2,2)]
		[Field ("HKPredicateKeyPathDateComponents")]
		NSString DateComponents { get; }

		[Watch (3,0), iOS (10,0)]
		[Field ("HKPredicateKeyPathCDATitle")]
		NSString CdaTitle { get; }

		[Watch (3,0), iOS (10,0)]
		[Field ("HKPredicateKeyPathCDAPatientName")]
		NSString CdaPatientName { get; }

		[Watch (3,0), iOS (10,0)]
		[Field ("HKPredicateKeyPathCDAAuthorName")]
		NSString CdaAuthorName { get; }

		[Watch (3,0), iOS (10,0)]
		[Field ("HKPredicateKeyPathCDACustodianName")]
		NSString CdaCustodianName { get; }

		[Watch (4, 0), iOS (11, 0)]
		[Field ("HKPredicateKeyPathWorkoutTotalFlightsClimbed")]
		NSString TotalFlightsClimbed { get; }

		[Watch (5, 0), iOS (12, 0)]
		[Field ("HKPredicateKeyPathSum")]
		NSString PathSum { get; }

		[NoWatch, iOS (12, 0)]
		[Field ("HKPredicateKeyPathClinicalRecordFHIRResourceIdentifier")]
		NSString ClinicalRecordFhirResourceIdentifier { get; }

		[NoWatch, iOS (12, 0)]
		[Field ("HKPredicateKeyPathClinicalRecordFHIRResourceType")]
		NSString ClinicalRecordFhirResourceType { get; }

		[Watch (6, 0), iOS (13, 0)]
		[Field ("HKPredicateKeyPathMin")]
		NSString Min { get; }

		[Watch (6, 0), iOS (13, 0)]
		[Field ("HKPredicateKeyPathAverage")]
		NSString Average { get; }

		[Watch (6, 0), iOS (13, 0)]
		[Field ("HKPredicateKeyPathMax")]
		NSString Max { get; }

		[Watch (6, 0), iOS (13, 0)]
		[Field ("HKPredicateKeyPathMostRecent")]
		NSString MostRecent { get; }

		[Watch (6, 0), iOS (13, 0)]
		[Field ("HKPredicateKeyPathMostRecentStartDate")]
		NSString MostRecentStartDate { get; }

		[Watch (6, 0), iOS (13, 0)]
		[Field ("HKPredicateKeyPathMostRecentEndDate")]
		NSString MostRecentEndDate { get; }

		[Watch (6, 0), iOS (13, 0)]
		[Field ("HKPredicateKeyPathMostRecentDuration")]
		NSString MostRecentDuration { get; }

		[Watch (6, 0), iOS (13, 0)]
		[Field ("HKPredicateKeyPathCount")]
		NSString PathCount { get; }

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKPredicateKeyPathAverageHeartRate")]
		NSString AverageHeartRate { get; }

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKPredicateKeyPathECGClassification")]
		NSString EcgClassification { get; }

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKPredicateKeyPathECGSymptomsStatus")]
		NSString EcgSymptomsStatus { get; }
	}

	[NoWatch] // headers says it's available but it's only usable from another, unavailable, type
	[iOS (10,0)]
	[Static]
	[Internal]
	interface HKDetailedCdaErrorKeys {
		[Field ("HKDetailedCDAValidationErrorKey")]
		NSString ValidationErrorKey { get; }
	}

	[NoWatch]
	[iOS (10,0)]
	[StrongDictionary ("HKDetailedCdaErrorKeys")]
	[Internal]
	interface HKDetailedCdaErrors {
		NSString ValidationError { get; }
	}

	[Watch (2,0)]
	[iOS (8,0)]
	[DisableDefaultCtor] // - (instancetype)init NS_UNAVAILABLE;
	[BaseType (typeof (HKSample))]
	interface HKCategorySample {
		[Export ("categoryType")]
		HKCategoryType CategoryType { get; }

		[Export ("value")]
		nint Value { get; }

		[Static]
		[Export ("categorySampleWithType:value:startDate:endDate:metadata:")]
		[EditorBrowsable (EditorBrowsableState.Advanced)] // this is not the one we want to be seen (compat only)
		HKCategorySample FromType (HKCategoryType type, nint value, NSDate startDate, NSDate endDate, [NullAllowed] NSDictionary metadata);

		[Static]
		[Wrap ("FromType (type, value, startDate, endDate, metadata.GetDictionary ())")]
		HKCategorySample FromType (HKCategoryType type, nint value, NSDate startDate, NSDate endDate, HKMetadata metadata);

		[Static]
		[Export ("categorySampleWithType:value:startDate:endDate:")]
		HKCategorySample FromType (HKCategoryType type, nint value, NSDate startDate, NSDate endDate);

		[iOS (9,0)]
		[Static]
		[Export ("categorySampleWithType:value:startDate:endDate:device:metadata:")]
		HKCategorySample FromType (HKCategoryType type, nint value, NSDate startDate, NSDate endDate, [NullAllowed] HKDevice device, [NullAllowed] NSDictionary<NSString,NSObject> metadata);
	}

	[Watch (3,0), iOS (10,0)]
	[BaseType (typeof(HKSample))]
	[Abstract] // as per docs
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKDocumentSample
	interface HKDocumentSample
	{
		[NoWatch] // HKDocumentType is iOS only, rdar #27865614
		[Export ("documentType", ArgumentSemantic.Strong)]
		HKDocumentType DocumentType { get; }
	}

	[NoWatch, iOS (10,0)]
	[BaseType (typeof(HKDocumentSample), Name = "HKCDADocumentSample")]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKCDADocumentSample
	interface HKCdaDocumentSample
	{
		[NullAllowed, Export ("document")]
		HKCdaDocument Document { get; }

		[NoWatch]
		[Static]
		[Export ("CDADocumentSampleWithData:startDate:endDate:metadata:validationError:")]
		[return: NullAllowed]
		HKCdaDocumentSample Create (NSData documentData, NSDate startDate, NSDate endDate, [NullAllowed] NSDictionary metadata, out NSError validationError);

		[Static, Wrap ("Create (documentData, startDate, endDate, metadata.GetDictionary (), out validationError)")]
		[return: NullAllowed]
		HKCdaDocumentSample Create (NSData documentData, NSDate startDate, NSDate endDate, HKMetadata metadata, out NSError validationError);
	}

	[Watch (3,0), iOS (10,0)]
	[BaseType (typeof(NSObject), Name = "HKCDADocument")]
	[DisableDefaultCtor] // as per docs
	interface HKCdaDocument
	{
		[NullAllowed, Export ("documentData", ArgumentSemantic.Copy)]
		NSData DocumentData { get; }

		[Export ("title")]
		string Title { get; }

		[Export ("patientName")]
		string PatientName { get; }

		[Export ("authorName")]
		string AuthorName { get; }

		[Export ("custodianName")]
		string CustodianName { get; }
	}

	[Watch (2,0)]
	[iOS (8,0)]
	[BaseType (typeof (HKSample))]
	[DisableDefaultCtor] // NSInvalidArgumentException: The -init method is not available on HKCorrelation
	interface HKCorrelation : NSSecureCoding {

		[Export ("objects")]
		NSSet Objects { get; }

		[Export ("objectsForType:")]
		NSSet GetObjects (HKObjectType objectType);
		
		[Export ("correlationType")]
		HKCorrelationType CorrelationType { get; }

		[Static, Export ("correlationWithType:startDate:endDate:objects:metadata:")]
		[EditorBrowsable (EditorBrowsableState.Advanced)] // this is not the one we want to be seen (compat only)
		HKCorrelation Create (HKCorrelationType correlationType, NSDate startDate, NSDate endDate, NSSet objects, [NullAllowed] NSDictionary metadata);

		[Static, Wrap ("Create (correlationType, startDate, endDate, objects, metadata.GetDictionary ())")]
		HKCorrelation Create (HKCorrelationType correlationType, NSDate startDate, NSDate endDate, NSSet objects, HKMetadata metadata);

		[Static, Export ("correlationWithType:startDate:endDate:objects:")]
		HKCorrelation Create (HKCorrelationType correlationType, NSDate startDate, NSDate endDate, NSSet objects);

		[iOS (9,0)]
		[Static]
		[Export ("correlationWithType:startDate:endDate:objects:device:metadata:")]
		HKCorrelation Create (HKCorrelationType correlationType, NSDate startDate, NSDate endDate, NSSet<HKSample> objects, [NullAllowed] HKDevice device, [NullAllowed] NSDictionary<NSString,NSObject> metadata);
	}

	delegate void HKCorrelationQueryResultHandler (HKCorrelationQuery query, HKCorrelation[] correlations, NSError error);

	[Watch (2,0)]
	[iOS (8,0)]
	[BaseType (typeof (HKQuery))]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKCorrelationQuery
	interface HKCorrelationQuery {
		[Export ("initWithType:predicate:samplePredicates:completion:")]
		NativeHandle Constructor (HKCorrelationType correlationType, [NullAllowed] NSPredicate predicate, [NullAllowed] NSDictionary samplePredicates, HKCorrelationQueryResultHandler completion);

		[Export ("correlationType", ArgumentSemantic.Copy)]
		HKCorrelationType CorrelationType { get; }

		[Export ("samplePredicates", ArgumentSemantic.Copy)]
		NSDictionary SamplePredicates { get; }
	}

	[Watch (2,0)]
	[iOS (8,0)]
	[BaseType (typeof (HKSampleType))]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKCorrelationType
	interface HKCorrelationType {

	}

	delegate void HKHealthStoreGetRequestStatusForAuthorizationToShareHandler (HKAuthorizationRequestStatus requestStatus, NSError error);
	delegate void HKHealthStoreRecoverActiveWorkoutSessionHandler (HKWorkoutSession session, NSError error);

	[Watch (2,0)]
	[iOS (8,0)]
	[BaseType (typeof (NSObject))]
	interface HKHealthStore {
		// FIXME NS_EXTENSION_UNAVAILABLE("Not available to extensions") ;
		[Static]
		[Export ("isHealthDataAvailable")]
		bool IsHealthDataAvailable { get; }

		[NoWatch, iOS (12, 0)]
		[Export ("supportsHealthRecords")]
		bool SupportsHealthRecords { get; }

		// FIXME NS_EXTENSION_UNAVAILABLE("Not available to extensions") ;
		[Export ("authorizationStatusForType:")]
		HKAuthorizationStatus GetAuthorizationStatus (HKObjectType type);

		// FIXME NS_EXTENSION_UNAVAILABLE("Not available to extensions") ;
		[Async]
		[Export ("requestAuthorizationToShareTypes:readTypes:completion:")]
		void RequestAuthorizationToShare ([NullAllowed] NSSet typesToShare, [NullAllowed] NSSet typesToRead, Action<bool, NSError> completion);

		// FIXME NS_EXTENSION_UNAVAILABLE("Not available to extensions") ;
		[Async]
		[Export ("saveObject:withCompletion:")]
		void SaveObject (HKObject obj, Action<bool, NSError> completion);

		// FIXME NS_EXTENSION_UNAVAILABLE("Not available to extensions") ;
		[Async]
		[Export ("saveObjects:withCompletion:")]
		void SaveObjects (HKObject[] objects, Action<bool, NSError> completion);

		// FIXME NS_EXTENSION_UNAVAILABLE("Not available to extensions") ;
		[Async]
		[Export ("deleteObject:withCompletion:")]
		void DeleteObject (HKObject obj, Action<bool, NSError> completion);

		[iOS (9,0)]
		[Async]
		[Export ("deleteObjects:withCompletion:")]
		void DeleteObjects (HKObject[] objects, Action<bool, NSError> completion);

		[iOS (9,0)]
		[Export ("deleteObjectsOfType:predicate:withCompletion:")]
		void DeleteObjects (HKObjectType objectType, NSPredicate predicate, Action<bool, nuint, NSError> completion);

		[iOS (9,0)]
		[Export ("earliestPermittedSampleDate")]
		NSDate EarliestPermittedSampleDate { get; }

		// FIXME NS_EXTENSION_UNAVAILABLE("Not available to extensions") ;
		[Export ("executeQuery:")]
		void ExecuteQuery (HKQuery query);

		[iOS (9,0)]
		[Export ("fitzpatrickSkinTypeWithError:")]
		[return: NullAllowed]
		HKFitzpatrickSkinTypeObject GetFitzpatrickSkinType (out NSError error);

		[Watch (3,0), iOS (10,0)]
		[Export ("wheelchairUseWithError:")]
		[return: NullAllowed]
		HKWheelchairUseObject GetWheelchairUse (out NSError error);

		[Watch (7,0), iOS (14,0)]
		[Export ("activityMoveModeWithError:")]
		[return: NullAllowed]
		HKActivityMoveModeObject GetActivityMoveMode ([NullAllowed] out NSError error);

		// FIXME NS_EXTENSION_UNAVAILABLE("Not available to extensions") ;
		[Export ("stopQuery:")]
		void StopQuery (HKQuery query);

		// FIXME NS_EXTENSION_UNAVAILABLE("Not available to extensions") ;
		[Deprecated (PlatformName.WatchOS, 3, 0, message: "Use 'GetDateOfBirthComponents' instead.")]
		[Deprecated (PlatformName.iOS, 10, 0, message: "Use 'GetDateOfBirthComponents' instead.")]
		[Export ("dateOfBirthWithError:")]
		NSDate GetDateOfBirth (out NSError error);

		[Watch (3,0), iOS (10,0)]
		[Export ("dateOfBirthComponentsWithError:")]
		[return: NullAllowed]
		NSDateComponents GetDateOfBirthComponents (out NSError error);

		// FIXME NS_EXTENSION_UNAVAILABLE("Not available to extensions") ;
		[Export ("biologicalSexWithError:")]
		HKBiologicalSexObject GetBiologicalSex (out NSError error);

		// FIXME NS_EXTENSION_UNAVAILABLE("Not available to extensions") ;
		[Export ("bloodTypeWithError:")]
		HKBloodTypeObject GetBloodType (out NSError error);

		[Watch (8,0)]
		[Async]
		[Export ("enableBackgroundDeliveryForType:frequency:withCompletion:")]
		void EnableBackgroundDelivery (HKObjectType type, HKUpdateFrequency frequency, Action<bool, NSError> completion);

		[Watch (8,0)]
		[Async]
		[Export ("disableBackgroundDeliveryForType:withCompletion:")]
		void DisableBackgroundDelivery (HKObjectType type, Action<bool, NSError> completion);

		[Watch (8,0)]
		[Async]
		[Export ("disableAllBackgroundDeliveryWithCompletion:")]
		void DisableAllBackgroundDelivery (Action<bool, NSError> completion);

		// FIXME NS_EXTENSION_UNAVAILABLE("Not available to extensions") ;
		[NoWatch]
		[Async]
		[iOS (9,0)]
		[Export ("handleAuthorizationForExtensionWithCompletion:")]
		void HandleAuthorizationForExtension (Action<bool, NSError> completion);

		[iOS (9,0)]
		[Deprecated (PlatformName.WatchOS, 4, 0)]
		[Deprecated (PlatformName.iOS, 11, 0)]
		[Export ("splitTotalEnergy:startDate:endDate:resultsHandler:")]
		void SplitTotalEnergy (HKQuantity totalEnergy, NSDate startDate, NSDate endDate, Action<HKQuantity, HKQuantity, NSError> resultsHandler);

		// HKWorkout category

		[Export ("addSamples:toWorkout:completion:")]
		void AddSamples (HKSample [] samples, HKWorkout workout, HKStoreSampleAddedCallback callback);

		[NoiOS]
		[Deprecated (PlatformName.WatchOS, 5, 0, message: "Use 'HKWorkoutSession.Start' instead.")]
		[Export ("startWorkoutSession:")]
		void StartWorkoutSession (HKWorkoutSession workoutSession);

		[NoiOS]
		[Deprecated (PlatformName.WatchOS, 5, 0, message: "Use 'HKWorkoutSession.End' instead.")]
		[Export ("endWorkoutSession:")]
		void EndWorkoutSession (HKWorkoutSession workoutSession);

		[Watch (3,0), NoiOS]
		[Deprecated (PlatformName.WatchOS, 5, 0, message: "Use 'HKWorkoutSession.Pause' instead.")]
		[Export ("pauseWorkoutSession:")]
		void PauseWorkoutSession (HKWorkoutSession workoutSession);

		[Watch (3,0), NoiOS]
		[Deprecated (PlatformName.WatchOS, 5, 0, message: "Use 'HKWorkoutSession.Resume' instead.")]
		[Export ("resumeWorkoutSession:")]
		void ResumeWorkoutSession (HKWorkoutSession workoutSession);

		[NoWatch, iOS (10,0)]
		[Async]
		[Export ("startWatchAppWithWorkoutConfiguration:completion:")]
		void StartWatchApp (HKWorkoutConfiguration workoutConfiguration, Action<bool, NSError> completion);

		// HKUserPreferences category

		[iOS (8,2)]
		[Async]
		[Export ("preferredUnitsForQuantityTypes:completion:")]
		void GetPreferredUnits (NSSet quantityTypes, Action<NSDictionary, NSError> completion);

		[iOS (8,2)]
		[Notification]
		[Field ("HKUserPreferencesDidChangeNotification")]
		NSString UserPreferencesDidChangeNotification { get; }

		[Async]
		[Watch (5,0), iOS (12,0)]
		[Export ("getRequestStatusForAuthorizationToShareTypes:readTypes:completion:")]
		void GetRequestStatusForAuthorizationToShare (NSSet<HKSampleType> typesToShare, NSSet<HKObjectType> typesToRead, HKHealthStoreGetRequestStatusForAuthorizationToShareHandler completion);

		[Async]
		[Watch (5,0), NoiOS]
		[Export ("recoverActiveWorkoutSessionWithCompletion:")]
		void RecoverActiveWorkoutSession (HKHealthStoreRecoverActiveWorkoutSessionHandler completion);

		[Async]
		[Watch (8,0), iOS (15,0)]
		[Export ("recalibrateEstimatesForSampleType:atDate:completion:")]
		void RecalibrateEstimates (HKSampleType sampleType, NSDate date, Action<bool, NSError> completion);
	}

	delegate void HKStoreSampleAddedCallback (bool success, NSError error);
	
	[Watch (2,0)]
	[iOS (8,0)]
	[BaseType (typeof (NSObject))]
	interface HKBiologicalSexObject : NSCopying, NSSecureCoding {
		[Export ("biologicalSex")]
		HKBiologicalSex BiologicalSex { get; }
	}

	[Watch (2,0)]
	[iOS (8,0)]
	[BaseType (typeof (NSObject))]
	interface HKBloodTypeObject : NSCopying, NSSecureCoding {
		[Export ("bloodType")]
		HKBloodType BloodType { get; }
	}

	[Watch (6,0)]
	[iOS (13,0)]
	[BaseType (typeof (HKSampleType))]
	[DisableDefaultCtor]
	interface HKAudiogramSampleType {}

	[StrongDictionary ("HKMetadataKey")]
	interface HKMetadata {
		[Export ("FoodType")]
		string FoodType { get; set; }

		[Export ("UdiDeviceIdentifier")]
		string UdiDeviceIdentifier { get; set; }

		[Export ("UdiProductionIdentifier")]
		string UdiProductionIdentifier { get; set; }

		[Export ("DigitalSignature")]
		string DigitalSignature { get; set; }

		[Export ("ExternalUuid")]
		string ExternalUuid { get; set; }

		[Export ("DeviceSerialNumber")]
		string DeviceSerialNumber { get; set; }

		[Export ("BodyTemperatureSensorLocation")]
		HKBodyTemperatureSensorLocation BodyTemperatureSensorLocation { get; set; }

		[Export ("HeartRateSensorLocation")]
		HKHeartRateSensorLocation HeartRateSensorLocation { get; set; }

		[Export ("TimeZone")]
		NSTimeZone TimeZone { get; set; }

		[Export ("DeviceName")]
		string DeviceName { get; set; }

		[Export ("DeviceManufacturerName")]
		string DeviceManufacturerName { get; set; }
		
		[Export ("WasTakenInLab")]
		bool WasTakenInLab { get; set; }

		[Export ("ReferenceRangeLowerLimit")]
		NSNumber ReferenceRangeLowerLimit { get; set; }
		
		[Export ("ReferenceRangeUpperLimit")]
		NSNumber ReferenceRangeUpperLimit { get; set; }
		
		[Export ("WasUserEntered")]
		bool WasUserEntered { get; set; }
		
		[Export ("WorkoutBrandName")]
		string WorkoutBrandName { get; set; }
		
		[Export ("GroupFitness")]
		bool GroupFitness { get; set; }
		
		[Export ("IndoorWorkout")]
		bool IndoorWorkout { get; set; }
		
		[Export ("CoachedWorkout")]
		bool CoachedWorkout { get; set; }

		[iOS (9,0)]
		[Export ("SexualActivityProtectionUsed")]
		bool SexualActivityProtectionUsed { get; set; }

		[iOS (9,0)]
		[Export ("MenstrualCycleStart")]
		bool MenstrualCycleStart { get; set; }

		[Watch (3,0), iOS (10,0)]
		[Export ("WeatherCondition")]
		HKWeatherCondition WeatherCondition { get; }

		[Watch (3,0), iOS (10,0)]
		[Export ("WeatherTemperature")]
		HKQuantity WeatherTemperature { get; }

		[Watch (3,0), iOS (10,0)]
		[Export ("WeatherHumidity")]
		HKQuantity WeatherHumidity { get; }

		[Watch (3,0), iOS (10,0)]
		[Export ("LapLength")]
		NSString LapLength { get; }

		[Watch (3,0), iOS (10,0)]
		[Export ("SwimmingLocationType")]
		NSString SwimmingLocationType { get; }

		[Watch (3,0), iOS (10,0)]
		[Export ("SwimmingStrokeStyle")]
		NSString SwimmingStrokeStyle { get; }

		[Watch (4, 0), iOS (11, 0)]
		[Export ("SyncIdentifier")]
		string SyncIdentifier { get; }

		[Watch (4, 0), iOS (11, 0)]
		[Export ("SyncVersion")]
		int SyncVersion { get; }

		[Watch (4, 0), iOS (11, 0)]
		[Export ("InsulinDeliveryReason")]
		HKInsulinDeliveryReason InsulinDeliveryReason { get; }

		[Watch (4, 0), iOS (11, 0)]
		[Export ("BloodGlucoseMealTime")]
		HKBloodGlucoseMealTime BloodGlucoseMealTime { get; }

		[Watch (4, 0), iOS (11, 0)]
		[Export ("VO2MaxTestType")]
		HKVO2MaxTestType VO2MaxTestType { get; }
        
		[Watch (4,0), iOS (11,0)]
		[Export ("HeartRateMotionContext")]
		HKHeartRateMotionContext HeartRateMotionContext { get; }

		[Watch (4,2), iOS (11,2)]
		[Export ("AverageSpeed")]
		HKQuantity AverageSpeed { get; set; }

		[Watch (4,2), iOS (11,2)]
		[Export ("MaximumSpeed")]
		HKQuantity MaximumSpeed { get; set; }

		[Watch (4,2), iOS (11,2)]
		[Export ("AlpineSlopeGrade")]
		HKQuantity AlpineSlopeGrade { get; set; }

		[Watch (4,2), iOS (11,2)]
		[Export ("ElevationAscended")]
		HKQuantity ElevationAscended { get; set; }

		[Watch (4,2), iOS (11,2)]
		[Export ("ElevationDescended")]
		HKQuantity ElevationDescended { get; set; }

		[Watch (5,0), iOS (12,0)]
		[Export ("FitnessMachineDuration")]
		HKQuantity FitnessMachineDuration { get; set; }

		[Watch (5, 0), iOS (12, 0)]
		[Export ("IndoorBikeDistance")]
		HKQuantity IndoorBikeDistance { get; set; }

		[Watch (5, 0), iOS (12, 0)]
		[Export ("CrossTrainerDistance")]
		HKQuantity CrossTrainerDistance { get; set; }

		[Watch (5, 2), iOS (12, 2)]
		[Export ("HeartRateEventThreshold")]
		HKQuantity HeartRateEventThreshold { get; set; }
	}
		
	[Watch (2,0)]
	[iOS (8,0)]
	[Static]
	interface HKMetadataKey {
		[Field ("HKMetadataKeyDeviceSerialNumber")]
		NSString DeviceSerialNumber { get; }

		[Field ("HKMetadataKeyBodyTemperatureSensorLocation")]
		NSString BodyTemperatureSensorLocation { get; }

		[Field ("HKMetadataKeyHeartRateSensorLocation")]
		NSString HeartRateSensorLocation { get; }

		[Field ("HKMetadataKeyFoodType")]
		NSString FoodType { get; }

		[Field ("HKMetadataKeyUDIDeviceIdentifier")]
		NSString UdiDeviceIdentifier { get; }

		[Field ("HKMetadataKeyUDIProductionIdentifier")]
		NSString UdiProductionIdentifier { get; }

		[Field ("HKMetadataKeyDigitalSignature")]
		NSString DigitalSignature { get; }

		[Field ("HKMetadataKeyExternalUUID")]
		NSString ExternalUuid { get; }

		[Field ("HKMetadataKeyTimeZone")]
		NSString TimeZone { get; }

		[Field ("HKMetadataKeyDeviceName")]
		NSString DeviceName { get; }

		[Field ("HKMetadataKeyDeviceManufacturerName")]
		NSString DeviceManufacturerName { get; }
		
		[Field ("HKMetadataKeyWasTakenInLab")]
		NSString WasTakenInLab { get; }

		[Field ("HKMetadataKeyReferenceRangeLowerLimit")]
		NSString ReferenceRangeLowerLimit { get; }

		[Field ("HKMetadataKeyReferenceRangeUpperLimit")]
		NSString ReferenceRangeUpperLimit { get; }
		
		[Field ("HKMetadataKeyWasUserEntered")]
		NSString WasUserEntered { get; }
		
		[Field ("HKMetadataKeyWorkoutBrandName")]
		NSString WorkoutBrandName { get; }
		
		[Field ("HKMetadataKeyGroupFitness")]
		NSString GroupFitness { get; }
		
		[Field ("HKMetadataKeyIndoorWorkout")]
		NSString IndoorWorkout { get; }
		
		[Field ("HKMetadataKeyCoachedWorkout")]
		NSString CoachedWorkout { get; }

		[iOS (9,0)]
		[Field ("HKMetadataKeySexualActivityProtectionUsed")]
		NSString SexualActivityProtectionUsed { get; }

		[iOS (9,0)]
		[Field ("HKMetadataKeyMenstrualCycleStart")]
		NSString MenstrualCycleStart { get; }

		[Watch (3,0), iOS (10,0)]
		[Field ("HKMetadataKeyWeatherCondition")]
		NSString WeatherCondition { get; }

		[Watch (3,0), iOS (10,0)]
		[Field ("HKMetadataKeyWeatherTemperature")]
		NSString WeatherTemperature { get; }

		[Watch (3,0), iOS (10,0)]
		[Field ("HKMetadataKeyWeatherHumidity")]
		NSString WeatherHumidity { get; }

		[Watch (3,0), iOS (10,0)]
		[Field ("HKMetadataKeyLapLength")]
		NSString LapLength { get; }

		[Watch (3,0), iOS (10,0)]
		[Field ("HKMetadataKeySwimmingLocationType")]
		NSString SwimmingLocationType { get; }

		[Watch (3,0), iOS (10,0)]
		[Field ("HKMetadataKeySwimmingStrokeStyle")]
		NSString SwimmingStrokeStyle { get; }

		[Watch (4, 0), iOS (11, 0)]
		[Field ("HKMetadataKeySyncIdentifier")]
		NSString SyncIdentifier { get; }

		[Watch (4, 0), iOS (11, 0)]
		[Field ("HKMetadataKeySyncVersion")]
		NSString SyncVersion { get; }

		[Watch (4, 0), iOS (11, 0)]
		[Field ("HKMetadataKeyInsulinDeliveryReason")]
		NSString InsulinDeliveryReason { get; }

		[Watch (4, 0), iOS (11, 0)]
		[Field ("HKMetadataKeyBloodGlucoseMealTime")]
		NSString BloodGlucoseMealTime { get; }

		[Watch (4, 0), iOS (11, 0)]
		[Field ("HKMetadataKeyVO2MaxTestType")]
		NSString VO2MaxTestType { get; }
        
		[Watch (4,0), iOS (11,0)]
		[Field ("HKMetadataKeyHeartRateMotionContext")]
		NSString HeartRateMotionContext { get; }

		[Watch (4,2), iOS (11,2)]
		[Field ("HKMetadataKeyAverageSpeed")]
		NSString AverageSpeed { get; }

		[Watch (4,2), iOS (11,2)]
		[Field ("HKMetadataKeyMaximumSpeed")]
		NSString MaximumSpeed { get; }

		[Watch (4,2), iOS (11,2)]
		[Field ("HKMetadataKeyAlpineSlopeGrade")]
		NSString AlpineSlopeGrade { get; }

		[Watch (4,2), iOS (11,2)]
		[Field ("HKMetadataKeyElevationAscended")]
		NSString ElevationAscended { get; }

		[Watch (4,2), iOS (11,2)]
		[Field ("HKMetadataKeyElevationDescended")]
		NSString ElevationDescended { get; }

		[Watch (5,0), iOS (12,0)]
		[Field ("HKMetadataKeyFitnessMachineDuration")]
		NSString FitnessMachineDuration { get; }

		[Watch (5, 0), iOS (12, 0)]
		[Field ("HKMetadataKeyIndoorBikeDistance")]
		NSString IndoorBikeDistance { get; }

		[Watch (5, 0), iOS (12, 0)]
		[Field ("HKMetadataKeyCrossTrainerDistance")]
		NSString CrossTrainerDistance { get; }

		[Watch (5, 2), iOS (12, 2)]
		[Field ("HKMetadataKeyHeartRateEventThreshold")]
		NSString HeartRateEventThreshold { get; }

		[Watch (6, 0), iOS (13, 0)]
		[Field ("HKMetadataKeyAverageMETs")]
		NSString AverageMets { get; }

		[Watch (6, 0), iOS (13, 0)]
		[Field ("HKMetadataKeyAudioExposureLevel")]
		NSString AudioExposureLevel { get; }

		[Watch (7, 1), iOS (14, 2)]
		[Field ("HKMetadataKeyAudioExposureDuration")]
		NSString AudioExposureDuration { get; }

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKMetadataKeyDevicePlacementSide")]
		NSString DevicePlacementSide { get; }

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKMetadataKeyBarometricPressure")]
		NSString BarometricPressure { get; }

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKMetadataKeyAppleECGAlgorithmVersion")]
		NSString AppleEcgAlgorithmVersion { get; }

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKMetadataKeyAppleDeviceCalibrated")]
		NSString AppleDeviceCalibrated { get; }

		[Watch (7, 2), iOS (14, 3)]
		[Field ("HKMetadataKeyVO2MaxValue")]
		NSString VO2MaxValue { get; }

		[Watch (7, 2), iOS (14, 3)]
		[Field ("HKMetadataKeyLowCardioFitnessEventThreshold")]
		NSString LowCardioFitnessEventThreshold { get; }

		[Watch (8, 0), iOS (15, 0)]
		[Field ("HKMetadataKeyDateOfEarliestDataUsedForEstimate")]
		NSString DateOfEarliestDataUsedForEstimate { get;} 

		[Watch (8, 0), iOS (15, 0)]
		[Field ("HKMetadataKeyAlgorithmVersion")]
		NSString AlgorithmVersion { get; }
	}

	[Watch (2,0)]
	[iOS (8,0)]
#if NET
	[Abstract] // as per docs
#endif
	[DisableDefaultCtor] // - (instancetype)init NS_UNAVAILABLE;
	[BaseType (typeof (NSObject))]
	interface HKObject : NSSecureCoding {
		[Export ("UUID", ArgumentSemantic.Strong)]
		NSUuid Uuid { get; }

		[Deprecated (PlatformName.iOS, 9, 0)]
		[Export ("source", ArgumentSemantic.Strong)]
		HKSource Source { get; }

		[Export ("metadata", ArgumentSemantic.Copy)]
		NSDictionary WeakMetadata { get; }

		[Wrap ("WeakMetadata")]
		HKMetadata Metadata { get; }

		[iOS (9,0)]
		[Export ("sourceRevision", ArgumentSemantic.Strong)]
		HKSourceRevision SourceRevision { get; }

		[iOS (9,0)]
		[NullAllowed, Export ("device", ArgumentSemantic.Strong)]
		HKDevice Device { get; }
	}

	[Watch (2,0)]
	[iOS (8,0)]
#if NET
	[Abstract]
#endif
	[DisableDefaultCtor] // - (instancetype)init NS_UNAVAILABLE;
	[BaseType (typeof (NSObject))]
	interface HKObjectType : NSSecureCoding, NSCopying {
		// These identifiers come from HKTypeIdentifiers
		[Export ("identifier")]
		NSString Identifier { get; }

#if NET || WATCH
		[Internal]
#else
		[Obsolete ("Use 'HKQuantityType.Create (HKQuantityTypeIdentifier)'.")]
#endif
		[Static]
		[Export ("quantityTypeForIdentifier:")]
		[return: NullAllowed]
		HKQuantityType GetQuantityType (NSString hkTypeIdentifier);

#if NET || WATCH
		[Internal]
#else
		[Obsolete ("Use 'HKCategoryType.Create (HKCategoryTypeIdentifier)'.")]
#endif
		[Static]
		[Export ("categoryTypeForIdentifier:")]
		[return: NullAllowed]
		HKCategoryType GetCategoryType (NSString hkCategoryTypeIdentifier);

#if NET || WATCH
		[Internal]
#else
		[Obsolete ("Use 'HKCharacteristicType.Create (HKCharacteristicTypeIdentifier)'.")]
#endif
		[Static]
		[Export ("characteristicTypeForIdentifier:")]
		[return: NullAllowed]
		HKCharacteristicType GetCharacteristicType (NSString hkCharacteristicTypeIdentifier);

#if NET || WATCH
		[Internal]
#else
		[Obsolete ("Use 'HKCorrelationType.Create (HKCorrelationTypeIdentifier)'.")]
#endif
		[Static, Export ("correlationTypeForIdentifier:")]
		[return: NullAllowed]
		HKCorrelationType GetCorrelationType (NSString hkCorrelationTypeIdentifier);

		[NoWatch] // HKDocumentType is iOS only, rdar #27865614
		[iOS (10,0)]
		[Internal]
		[Static]
		[Export ("documentTypeForIdentifier:")]
		[return: NullAllowed]
		HKDocumentType _GetDocumentType (NSString hkDocumentTypeIdentifier);

		[Static, Export ("workoutType")]
#if NET
		HKWorkoutType WorkoutType { get; }
#else
		HKWorkoutType GetWorkoutType ();
#endif

		[Watch (2,2)]
		[iOS (9,3)]
		[Static]
		[Export ("activitySummaryType")]
		HKActivitySummaryType ActivitySummaryType { get; }

		[Watch (4, 0), iOS (11, 0)]
		[Static]
		[Export ("seriesTypeForIdentifier:")]
		[return: NullAllowed]
		HKSeriesType GetSeriesType (string identifier);

		[Watch (5,0), iOS (12,0)]
		[Static, Internal]
		[Export ("clinicalTypeForIdentifier:")]
		[return: NullAllowed]
		HKClinicalType GetClinicalType (NSString identifier);

		[Watch (5,0), iOS (12,0)]
		[Static]
		[Wrap ("GetClinicalType (identifier.GetConstant ()!)")]
		[return: NullAllowed]
		HKClinicalType GetClinicalType (HKClinicalTypeIdentifier identifier);

		[Watch (6, 0), iOS (13, 0)]
		[Static]
		[Export ("audiogramSampleType")]
		HKAudiogramSampleType AudiogramSampleType { get; }

		[Watch (7, 0), iOS (14, 0)]
		[Static]
		[Export ("electrocardiogramType")]
		HKElectrocardiogramType ElectrocardiogramType { get; }
	}

	[Watch (7, 0), iOS (14, 0)]
	[BaseType (typeof (HKSampleType))]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKElectrocardiogram
	interface HKElectrocardiogramType {

	}

	[Watch (2,0)]
	[iOS (8,0)]
	[BaseType (typeof (HKObjectType))]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKCharacteristicType
	interface HKCharacteristicType {

	}

	[Watch (2,0)]
	[iOS (8,0)]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKSampleType
	[BaseType (typeof (HKObjectType))]
	[Abstract] // The HKSampleType class is an abstract subclass of the HKObjectType class, used to represent data samples. Never instantiate an HKSampleType object directly. Instead, you should always work with one of its concrete subclasses [...]
	interface HKSampleType {
		[iOS (13,0), Watch (6,0)]
		[Export ("isMaximumDurationRestricted")]
		bool IsMaximumDurationRestricted { get; }

		[iOS (13,0), Watch (6,0)]
		[Export ("maximumAllowedDuration")]
		double MaximumAllowedDuration { get; }

		[iOS (13,0), Watch (6,0)]
		[Export ("isMinimumDurationRestricted")]
		bool IsMinimumDurationRestricted { get; }

		[iOS (13,0), Watch (6,0)]
		[Export ("minimumAllowedDuration")]
		double MinimumAllowedDuration { get; }

		[Watch (8, 0), iOS (15, 0)]
		[Export ("allowsRecalibrationForEstimates")]
		bool AllowsRecalibrationForEstimates { get; }
	}

	[Watch (5,0)]
	[iOS (12,0)]
	[BaseType (typeof (HKSampleType))]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKClinicalType
	interface HKClinicalType {

	}

	[Watch (2,0)]
	[iOS (8,0)]
	[BaseType (typeof (HKSampleType))]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKCategoryType
	interface HKCategoryType {

	}

	[NoWatch] // marked as iOS-only (confirmed by Apple) even if some watchOS 3 API returns this type, rdar #27865614
	[iOS (10,0)]
	[BaseType (typeof (HKSampleType))]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKDocumentType
	interface HKDocumentType {

	}

	[Watch (2,0)]
	[iOS (8,0)]
	[BaseType (typeof (HKSampleType))]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKQuantityType
	interface HKQuantityType {
		[Export ("aggregationStyle")]
		HKQuantityAggregationStyle AggregationStyle { get; }

		[Export ("isCompatibleWithUnit:")]
		bool IsCompatible (HKUnit unit);
	}

	delegate void HKObserverQueryUpdateHandler (HKObserverQuery query, [BlockCallback] Action completion, NSError error);

	[Watch (8,0), iOS (15,0)]
	delegate void HKObserverQueryDescriptorUpdateHandler (HKObserverQuery query, NSSet<HKSampleType> samples, [BlockCallback] Action completion, NSError error);

	[Watch (2,0)]
	[iOS (8,0)]
	[BaseType (typeof (HKQuery))]
#if NET
	[Abstract]
#endif
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKObserverQuery
	interface HKObserverQuery {
		[Export ("initWithSampleType:predicate:updateHandler:")]
		NativeHandle Constructor (HKSampleType sampleType, [NullAllowed] NSPredicate predicate, HKObserverQueryUpdateHandler updateHandler);

		[Watch (8,0), iOS (15,0)]
		[Export ("initWithQueryDescriptors:updateHandler:")]
		NativeHandle Constructor (HKQueryDescriptor[] queryDescriptors, HKObserverQueryDescriptorUpdateHandler updateHandler);
	}

	[Watch (2,0)]
	[iOS (8,0)]
	[DisableDefaultCtor] // - (instancetype)init NS_UNAVAILABLE;
	[BaseType (typeof (NSObject))]
	interface HKQuantity : NSSecureCoding, NSCopying {
		[Static]
		[Export ("quantityWithUnit:doubleValue:")]
		HKQuantity FromQuantity (HKUnit unit, double value);

		[Export ("isCompatibleWithUnit:")]
		bool IsCompatible (HKUnit unit);

		[Export ("doubleValueForUnit:")]
		double GetDoubleValue (HKUnit unit);

		[Export ("compare:")]
		NSComparisonResult Compare (HKQuantity quantity);
	}

	[Watch (2,0)]
	[iOS (8,0)]
	[BaseType (typeof (HKSample))]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKQuantitySample
	interface HKQuantitySample {
		[Export ("quantityType", ArgumentSemantic.Strong)]
		HKQuantityType QuantityType { get; }

		[Export ("quantity", ArgumentSemantic.Strong)]
		HKQuantity Quantity { get; }

		[Static]
		[Export ("quantitySampleWithType:quantity:startDate:endDate:")]
		HKQuantitySample FromType (HKQuantityType quantityType, HKQuantity quantity, NSDate startDate, NSDate endDate);

		[Static]
		[Export ("quantitySampleWithType:quantity:startDate:endDate:metadata:")]
		[EditorBrowsable (EditorBrowsableState.Advanced)] // this is not the one we want to be seen (compat only)
		HKQuantitySample FromType (HKQuantityType quantityType, HKQuantity quantity, NSDate startDate, NSDate endDate, [NullAllowed] NSDictionary metadata);

		[Static]
		[Wrap ("FromType (quantityType, quantity, startDate, endDate, metadata.GetDictionary ())")]
		HKQuantitySample FromType (HKQuantityType quantityType, HKQuantity quantity, NSDate startDate, NSDate endDate, HKMetadata metadata);

		[iOS (9,0)]
		[Static]
		[Export ("quantitySampleWithType:quantity:startDate:endDate:device:metadata:")]
		HKQuantitySample FromType (HKQuantityType quantityType, HKQuantity quantity, NSDate startDate, NSDate endDate, [NullAllowed] HKDevice device, [NullAllowed] NSDictionary<NSString,NSObject> metadata);

		[Watch (5, 0), iOS (12, 0)]
		[Export ("count")]
		nint Count { get; }
	}

	[Watch (2,0)]
	[iOS (8,0)]
	[DisableDefaultCtor] // - (instancetype)init NS_UNAVAILABLE;
	[BaseType (typeof (NSObject))]
	interface HKQuery {
		[iOS (9,3), Watch (2,2)]
		[NullAllowed, Export ("objectType", ArgumentSemantic.Strong)]
		HKObjectType ObjectType { get; }

		[Deprecated (PlatformName.WatchOS, 2,2, message: "Use 'ObjectType' property.")]
		[Deprecated (PlatformName.iOS, 9,3, message: "Use 'ObjectType' property.")]
		[NullAllowed, Export ("sampleType", ArgumentSemantic.Strong)]
		HKSampleType SampleType { get; }

		[Export ("predicate", ArgumentSemantic.Strong)]
		NSPredicate Predicate { get; }

		// HKQuery (HKObjectPredicates) Category

		[Static]
		[Export ("predicateForObjectsWithMetadataKey:")]
		NSPredicate GetPredicateForMetadataKey (NSString metadataKey);

		[Static]
		[Export ("predicateForObjectsWithMetadataKey:allowedValues:")]
		NSPredicate GetPredicateForMetadataKey (NSString metadataKey, NSObject[] allowedValues);

		[Static]
		[Export ("predicateForObjectsWithMetadataKey:operatorType:value:")]
		NSPredicate GetPredicateForMetadataKey (NSString metadataKey, NSPredicateOperatorType operatorType, NSObject value);

		[Static]
		[Export ("predicateForObjectsFromSource:")]
		NSPredicate GetPredicateForObjectsFromSource (HKSource source);

		[Static]
		[Export ("predicateForObjectsFromSources:")]
		NSPredicate GetPredicateForObjectsFromSources (NSSet sources);

		[Static]
		[Export ("predicateForObjectWithUUID:")]
		NSPredicate GetPredicateForObject (NSUuid objectUuid);

		[Static]
		[Export ("predicateForObjectsWithUUIDs:")]
		NSPredicate GetPredicateForObjects (NSSet objectUuids);

		[iOS (9,0)]
		[Static]
		[Export ("predicateForObjectsFromDevices:")]
		NSPredicate GetPredicateForObjectsFromDevices (NSSet<HKDevice> devices);

		[iOS (9,0)]
		[Static]
		[Export ("predicateForObjectsWithDeviceProperty:allowedValues:")]
		NSPredicate GetPredicateForObjectsWithDeviceProperty (string key, NSSet<NSString> allowedValues);

		[iOS (9,0)]
		[Static]
		[Export ("predicateForObjectsFromSourceRevisions:")]
		NSPredicate GetPredicateForObjectsFromSourceRevisions (NSSet<HKSourceRevision> sourceRevisions);

		[Watch (7,0), iOS (14,0)]
		[Static]
		[Export ("predicateForObjectsAssociatedWithElectrocardiogram:")]
		NSPredicate GetPredicateForObjects (HKElectrocardiogram electrocardiogram);

		// HKQuery (HKQuantitySamplePredicates) Category

		[Static]
		[Export ("predicateForQuantitySamplesWithOperatorType:quantity:")]
		NSPredicate GetPredicateForQuantitySamples (NSPredicateOperatorType operatorType, HKQuantity quantity);

		// HKQuery (HKCategorySamplePredicates) Category

		[Static]
		[Export ("predicateForCategorySamplesWithOperatorType:value:")]
		NSPredicate GetPredicateForCategorySamples (NSPredicateOperatorType operatorType, nint value);

		[Static]
		[Export ("predicateForSamplesWithStartDate:endDate:options:")]
		NSPredicate GetPredicateForSamples ([NullAllowed] NSDate startDate, [NullAllowed] NSDate endDate, HKQueryOptions options);

		[Static]
		[Export ("predicateForObjectsWithNoCorrelation")]
		NSPredicate PredicateForObjectsWithNoCorrelation ();

		[Static]
		[Export ("predicateForObjectsFromWorkout:")]
		NSPredicate GetPredicateForObjectsFromWorkout (HKWorkout workout);

		[Static]
		[Export ("predicateForWorkoutsWithWorkoutActivityType:")]
		NSPredicate GetPredicateForWorkouts (HKWorkoutActivityType workoutActivityType);

		[Static]
		[Export ("predicateForWorkoutsWithOperatorType:duration:")]
		NSPredicate GetPredicateForDuration (NSPredicateOperatorType operatorType, double duration);

		[Static]
		[Export ("predicateForWorkoutsWithOperatorType:totalEnergyBurned:")]
		NSPredicate GetPredicateForTotalEnergyBurned (NSPredicateOperatorType operatorType, HKQuantity totalEnergyBurned);

		[Static]
		[Export ("predicateForWorkoutsWithOperatorType:totalDistance:")]
		NSPredicate GetPredicateForTotalDistance (NSPredicateOperatorType operatorType, HKQuantity totalDistance);

		[iOS (10,0), Watch (3,0)]
		[Static]
		[Export ("predicateForWorkoutsWithOperatorType:totalSwimmingStrokeCount:")]
		NSPredicate GetPredicateForTotalSwimmingStrokeCount (NSPredicateOperatorType operatorType, HKQuantity totalSwimmingStrokeCount);

		[Watch (4, 0), iOS (11, 0)]
		[Static]
		[Export ("predicateForWorkoutsWithOperatorType:totalFlightsClimbed:")]
		NSPredicate GetPredicateForTotalFlightsClimbed (NSPredicateOperatorType operatorType, HKQuantity totalFlightsClimbed);

		// HKActivitySummaryPredicates

		[iOS (9,3), Watch (2,2)]
		[Static]
		[Export ("predicateForActivitySummaryWithDateComponents:")]
		NSPredicate GetPredicateForActivitySummary (NSDateComponents dateComponents);

		[iOS (9,3), Watch (2,2)]
		[Static]
		[Export ("predicateForActivitySummariesBetweenStartDateComponents:endDateComponents:")]
		NSPredicate GetPredicateForActivitySummariesBetween (NSDateComponents startDateComponents, NSDateComponents endDateComponents);


		// @interface HKClinicalRecordPredicates (HKQuery)
		[NoWatch, iOS (12,0)]
		[Static, Internal]
		[Export ("predicateForClinicalRecordsWithFHIRResourceType:")]
		NSPredicate GetPredicateForClinicalRecords (NSString resourceType);

		[NoWatch, iOS (12,0)]
		[Static]
		[Wrap ("GetPredicateForClinicalRecords (resourceType.GetConstant ()!)")]
		NSPredicate GetPredicateForClinicalRecords (HKFhirResourceType resourceType);

		[NoWatch, iOS (12,0)]
		[Static, Internal]
		[Export ("predicateForClinicalRecordsFromSource:FHIRResourceType:identifier:")]
		NSPredicate GetPredicateForClinicalRecords (HKSource source, string resourceType, string identifier);

		[NoWatch, iOS (12,0)]
		[Static]
		[Wrap ("GetPredicateForClinicalRecords (source, resourceType.GetConstant (), identifier)")]
		NSPredicate GetPredicateForClinicalRecords (HKSource source, HKFhirResourceType resourceType, string identifier);

		// @interface HKElectrocardiogramPredicates (HKQuery)

		[Watch (7,0), iOS (14,0)]
		[Static]
		[Export ("predicateForElectrocardiogramsWithClassification:")]
		NSPredicate GetPredicateForElectrocardiograms (HKElectrocardiogramClassification classification);

		[Watch (7,0), iOS (14,0)]
		[Static]
		[Export ("predicateForElectrocardiogramsWithSymptomsStatus:")]
		NSPredicate GetPredicateForElectrocardiograms (HKElectrocardiogramSymptomsStatus symptomsStatus);

		// @interface HKVerifiableClinicalRecordPredicates (HKQuery)
		[iOS (15,0), Watch (8,0)]
		[Static]
		[Export ("predicateForVerifiableClinicalRecordsWithRelevantDateWithinDateInterval:")]
		NSPredicate GetPredicateForVerifiableClinicalRecords (NSDateInterval dateInterval);
	}

	[Watch (2,0)]
	[iOS (8,0)]
	[BaseType (typeof (HKObject))]
#if NET
	[Abstract]
#endif
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKSample
	interface HKSample {

		[Export ("sampleType", ArgumentSemantic.Strong)]
		HKSampleType SampleType { get; }

		[Export ("startDate", ArgumentSemantic.Strong)]
		NSDate StartDate { get; }

		[Export ("endDate", ArgumentSemantic.Strong)]
		NSDate EndDate { get; }

		// TODO: where is this thing used?
		[Field ("HKSampleSortIdentifierStartDate")]
		NSString SortIdentifierStartDate { get; }

		// TODO: where is this thing used?
		[Field ("HKSampleSortIdentifierEndDate")]
		NSString SortIdentifierEndDate { get; }

		[Watch (7, 2), iOS (14, 3)]
		[Export ("hasUndeterminedDuration")]
		bool HasUndeterminedDuration { get; }
	}

	delegate void HKSampleQueryResultsHandler (HKSampleQuery query, HKSample [] results, NSError error);

	[Watch (2,0)]
	[iOS (8,0)]
	[BaseType (typeof (HKQuery))]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKSampleQuery
	interface HKSampleQuery {

		[Export ("limit")]
		nuint Limit { get; }

		[Export ("sortDescriptors")]
		NSSortDescriptor[] SortDescriptors { get; }

		[Export ("initWithSampleType:predicate:limit:sortDescriptors:resultsHandler:")]
		NativeHandle Constructor (HKSampleType sampleType, [NullAllowed] NSPredicate predicate, nuint limit, [NullAllowed] NSSortDescriptor[] sortDescriptors, HKSampleQueryResultsHandler resultsHandler);

		[Watch (8,0), iOS (15,0)]
		[Export ("initWithQueryDescriptors:limit:resultsHandler:")]
		NativeHandle Constructor (HKQueryDescriptor[] queryDescriptors, nint limit, HKSampleQueryResultsHandler resultsHandler);

		[Watch (8,0), iOS (15,0)]
		[Export ("initWithQueryDescriptors:limit:sortDescriptors:resultsHandler:")]
		NativeHandle Constructor (HKQueryDescriptor[] queryDescriptors, nint limit, NSSortDescriptor[] sortDescriptors, HKSampleQueryResultsHandler resultsHandler);
	}

	[Watch (2,0)]
	[iOS (8,0)]
	[DisableDefaultCtor] // - (instancetype)init NS_UNAVAILABLE;
	[BaseType (typeof (NSObject))]
	interface HKSource : NSSecureCoding, NSCopying {
		[Export ("name")]
		string Name { get; }

		[Export ("bundleIdentifier")]
		string BundleIdentifier { get; }

		[Static]
		[Export ("defaultSource")]
		HKSource GetDefaultSource { get; }
	}

	delegate void HKSourceQueryCompletionHandler (HKSourceQuery query, NSSet sources, NSError error);

	[Watch (2,0)]
	[iOS (8,0)]
	[BaseType (typeof (HKQuery))]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKSourceQuery
	interface HKSourceQuery {

		[Export ("initWithSampleType:samplePredicate:completionHandler:")]
		NativeHandle Constructor (HKSampleType sampleType, [NullAllowed] NSPredicate objectPredicate, HKSourceQueryCompletionHandler completionHandler);
	}

	[Watch (2,0)]
	[iOS (8,0)]
	[DisableDefaultCtor] // - (instancetype)init NS_UNAVAILABLE;
	[BaseType (typeof (NSObject))]
	interface HKStatistics : NSSecureCoding, NSCopying {
		[Export ("quantityType", ArgumentSemantic.Strong)]
		HKQuantityType QuantityType { get; }

		[Export ("startDate", ArgumentSemantic.Strong)]
		NSDate StartDate { get; }

		[Export ("endDate", ArgumentSemantic.Strong)]
		NSDate EndDate { get; }

		[Export ("sources")]
		HKSource [] Sources { get; }

		[Export ("averageQuantityForSource:")]
		HKQuantity AverageQuantity (HKSource source);

		[Export ("averageQuantity")]
		HKQuantity AverageQuantity ();

		[Export ("minimumQuantityForSource:")]
		HKQuantity MinimumQuantity (HKSource source);

		[Export ("minimumQuantity")]
		HKQuantity MinimumQuantity ();

		[Export ("maximumQuantityForSource:")]
		HKQuantity MaximumQuantity (HKSource source);

		[Export ("maximumQuantity")]
		HKQuantity MaximumQuantity ();

		[Export ("sumQuantityForSource:")]
		HKQuantity SumQuantity (HKSource source);

		[Export ("sumQuantity")]
		HKQuantity SumQuantity ();

		[Watch (5,0), iOS (12,0)]
		[Export ("mostRecentQuantityForSource:")]
		[return: NullAllowed]
		HKQuantity GetMostRecentQuantity (HKSource source);

		[Watch (5, 0), iOS (12, 0)]
		[NullAllowed, Export ("mostRecentQuantity")]
		HKQuantity MostRecentQuantity { get; }

		[Watch (5,0), iOS (12,0)]
		[Export ("mostRecentQuantityDateIntervalForSource:")]
		[return: NullAllowed]
		NSDateInterval GetMostRecentQuantityDateInterval (HKSource source);

		[Watch (5, 0), iOS (12, 0)]
		[NullAllowed, Export ("mostRecentQuantityDateInterval")]
		NSDateInterval MostRecentQuantityDateInterval { get; }

		[Watch (6, 0), iOS (13, 0)]
		[NullAllowed, Export ("duration")]
		HKQuantity Duration { get; }

		[Watch (6,0), iOS (13,0)]
		[Export ("durationForSource:")]
		[return: NullAllowed]
		HKQuantity GetDuration (HKSource source);
	}

	delegate void HKStatisticsCollectionEnumerator (HKStatistics result, bool stop);

	[Watch (2,0)]
	[iOS (8,0)]
	[DisableDefaultCtor] // - (instancetype)init NS_UNAVAILABLE;
	[BaseType (typeof (NSObject))]
	interface HKStatisticsCollection {

		[Export ("statisticsForDate:")]
		HKStatistics GetStatistics (NSDate date);

		[Export ("enumerateStatisticsFromDate:toDate:withBlock:")]
		void EnumerateStatistics (NSDate startDate, NSDate endDate, HKStatisticsCollectionEnumerator handler);

		[Export ("statistics")]
		HKStatistics[] Statistics { get; }

		[Export ("sources")]
		NSSet Sources { get; }
	}

	delegate void HKStatisticsCollectionQueryInitialResultsHandler (HKStatisticsCollectionQuery query, HKStatisticsCollection result, NSError error);
	delegate void HKStatisticsCollectionQueryStatisticsUpdateHandler (HKStatisticsCollectionQuery query, HKStatistics statistics, HKStatisticsCollection collection, NSError error);


	[Watch (2,0)]
	[iOS (8,0)]
	[BaseType (typeof (HKQuery))]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKStatisticsCollectionQuery
	interface HKStatisticsCollectionQuery {

		[Export ("anchorDate", ArgumentSemantic.Strong)]
		NSDate AnchorDate { get; }

		[Export ("options")]
		HKStatisticsOptions Options { get; }

		[Export ("intervalComponents", ArgumentSemantic.Copy)]
		NSDateComponents IntervalComponents { get; }

		[NullAllowed, Export ("initialResultsHandler", ArgumentSemantic.Copy)]
		HKStatisticsCollectionQueryInitialResultsHandler InitialResultsHandler { get; set; }

		[NullAllowed, Export ("statisticsUpdateHandler", ArgumentSemantic.Copy)]
		HKStatisticsCollectionQueryStatisticsUpdateHandler StatisticsUpdated { get; set; }

		[Export ("initWithQuantityType:quantitySamplePredicate:options:anchorDate:intervalComponents:")]
		NativeHandle Constructor (HKQuantityType quantityType, [NullAllowed] NSPredicate quantitySamplePredicate, HKStatisticsOptions options, NSDate anchorDate, NSDateComponents intervalComponents);
	}

	delegate void HKStatisticsQueryHandler (HKStatisticsQuery query, HKStatistics result, NSError error);

	[Watch (2,0)]
	[iOS (8,0)]
	[BaseType (typeof (HKQuery))]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKStatisticsQuery
	interface HKStatisticsQuery {

		[Export ("initWithQuantityType:quantitySamplePredicate:options:completionHandler:")]
		NativeHandle Constructor (HKQuantityType quantityType, [NullAllowed] NSPredicate quantitySamplePredicate, HKStatisticsOptions options, HKStatisticsQueryHandler handler);
	}

	[Watch (2,0)]
	[iOS (8,0)]
	enum HKQuantityTypeIdentifier {

		[Field ("HKQuantityTypeIdentifierBodyMassIndex")]
		BodyMassIndex,

		[Field ("HKQuantityTypeIdentifierBodyFatPercentage")]
		BodyFatPercentage,

		[Field ("HKQuantityTypeIdentifierHeight")]
		Height,

		[Field ("HKQuantityTypeIdentifierBodyMass")]
		BodyMass,

		[Field ("HKQuantityTypeIdentifierLeanBodyMass")]
		LeanBodyMass,

		[Field ("HKQuantityTypeIdentifierHeartRate")]
		HeartRate,

		[Field ("HKQuantityTypeIdentifierStepCount")]
		StepCount,

		[Field ("HKQuantityTypeIdentifierDistanceWalkingRunning")]
		DistanceWalkingRunning,

		[Field ("HKQuantityTypeIdentifierDistanceCycling")]
		DistanceCycling,

		[Field ("HKQuantityTypeIdentifierBasalEnergyBurned")]
		BasalEnergyBurned,

		[Field ("HKQuantityTypeIdentifierActiveEnergyBurned")]
		ActiveEnergyBurned,

		[Field ("HKQuantityTypeIdentifierFlightsClimbed")]
		FlightsClimbed,

		[Field ("HKQuantityTypeIdentifierNikeFuel")]
		NikeFuel,

		// Blood
		[Field ("HKQuantityTypeIdentifierOxygenSaturation")]
		OxygenSaturation,

		[Field ("HKQuantityTypeIdentifierBloodGlucose")]
		BloodGlucose,

		[Field ("HKQuantityTypeIdentifierBloodPressureSystolic")]
		BloodPressureSystolic,

		[Field ("HKQuantityTypeIdentifierBloodPressureDiastolic")]
		BloodPressureDiastolic,

		[Field ("HKQuantityTypeIdentifierBloodAlcoholContent")]
		BloodAlcoholContent,

		[Field ("HKQuantityTypeIdentifierPeripheralPerfusionIndex")]
		PeripheralPerfusionIndex,

		[Field ("HKQuantityTypeIdentifierForcedVitalCapacity")]
		ForcedVitalCapacity,

		[Field ("HKQuantityTypeIdentifierForcedExpiratoryVolume1")]
		ForcedExpiratoryVolume1,

		[Field ("HKQuantityTypeIdentifierPeakExpiratoryFlowRate")]
		PeakExpiratoryFlowRate,

		// Miscellaneous
		[Field ("HKQuantityTypeIdentifierNumberOfTimesFallen")]
		NumberOfTimesFallen,

		[Field ("HKQuantityTypeIdentifierInhalerUsage")]
		InhalerUsage,

		[Field ("HKQuantityTypeIdentifierRespiratoryRate")]
		RespiratoryRate,

		[Field ("HKQuantityTypeIdentifierBodyTemperature")]
		BodyTemperature,

		// Nutrition
		[Field ("HKQuantityTypeIdentifierDietaryFatTotal")]
		DietaryFatTotal,

		[Field ("HKQuantityTypeIdentifierDietaryFatPolyunsaturated")]
		DietaryFatPolyunsaturated,

		[Field ("HKQuantityTypeIdentifierDietaryFatMonounsaturated")]
		DietaryFatMonounsaturated,

		[Field ("HKQuantityTypeIdentifierDietaryFatSaturated")]
		DietaryFatSaturated,

		[Field ("HKQuantityTypeIdentifierDietaryCholesterol")]
		DietaryCholesterol,

		[Field ("HKQuantityTypeIdentifierDietarySodium")]
		DietarySodium,

		[Field ("HKQuantityTypeIdentifierDietaryCarbohydrates")]
		DietaryCarbohydrates,

		[Field ("HKQuantityTypeIdentifierDietaryFiber")]
		DietaryFiber,

		[Field ("HKQuantityTypeIdentifierDietarySugar")]
		DietarySugar,

		[Field ("HKQuantityTypeIdentifierDietaryEnergyConsumed")]
		DietaryEnergyConsumed,

		[Field ("HKQuantityTypeIdentifierDietaryProtein")]
		DietaryProtein,

		[Field ("HKQuantityTypeIdentifierDietaryVitaminA")]
		DietaryVitaminA,

		[Field ("HKQuantityTypeIdentifierDietaryVitaminB6")]
		DietaryVitaminB6,

		[Field ("HKQuantityTypeIdentifierDietaryVitaminB12")]
		DietaryVitaminB12,

		[Field ("HKQuantityTypeIdentifierDietaryVitaminC")]
		DietaryVitaminC,

		[Field ("HKQuantityTypeIdentifierDietaryVitaminD")]
		DietaryVitaminD,

		[Field ("HKQuantityTypeIdentifierDietaryVitaminE")]
		DietaryVitaminE,

		[Field ("HKQuantityTypeIdentifierDietaryVitaminK")]
		DietaryVitaminK,

		[Field ("HKQuantityTypeIdentifierDietaryCalcium")]
		DietaryCalcium,

		[Field ("HKQuantityTypeIdentifierDietaryIron")]
		DietaryIron,

		[Field ("HKQuantityTypeIdentifierDietaryThiamin")]
		DietaryThiamin,

		[Field ("HKQuantityTypeIdentifierDietaryRiboflavin")]
		DietaryRiboflavin,

		[Field ("HKQuantityTypeIdentifierDietaryNiacin")]
		DietaryNiacin,

		[Field ("HKQuantityTypeIdentifierDietaryFolate")]
		DietaryFolate,

		[Field ("HKQuantityTypeIdentifierDietaryBiotin")]
		DietaryBiotin,

		[Field ("HKQuantityTypeIdentifierDietaryPantothenicAcid")]
		DietaryPantothenicAcid,

		[Field ("HKQuantityTypeIdentifierDietaryPhosphorus")]
		DietaryPhosphorus,

		[Field ("HKQuantityTypeIdentifierDietaryIodine")]
		DietaryIodine,

		[Field ("HKQuantityTypeIdentifierDietaryMagnesium")]
		DietaryMagnesium,

		[Field ("HKQuantityTypeIdentifierDietaryZinc")]
		DietaryZinc,

		[Field ("HKQuantityTypeIdentifierDietarySelenium")]
		DietarySelenium,

		[Field ("HKQuantityTypeIdentifierDietaryCopper")]
		DietaryCopper,

		[Field ("HKQuantityTypeIdentifierDietaryManganese")]
		DietaryManganese,

		[Field ("HKQuantityTypeIdentifierDietaryChromium")]
		DietaryChromium,

		[Field ("HKQuantityTypeIdentifierDietaryMolybdenum")]
		DietaryMolybdenum,

		[Field ("HKQuantityTypeIdentifierDietaryChloride")]
		DietaryChloride,

		[Field ("HKQuantityTypeIdentifierDietaryPotassium")]
		DietaryPotassium,

		[Field ("HKQuantityTypeIdentifierDietaryCaffeine")]
		DietaryCaffeine,

		[iOS (9,0)]
		[Field ("HKQuantityTypeIdentifierBasalBodyTemperature")]
		BasalBodyTemperature,

		[iOS (9,0)]
		[Field ("HKQuantityTypeIdentifierDietaryWater")]
		DietaryWater,

		[iOS (9,0)]
		[Field ("HKQuantityTypeIdentifierUVExposure")]
		UVExposure,

		[Field ("HKQuantityTypeIdentifierElectrodermalActivity")]
		ElectrodermalActivity,

		[iOS (9,3), Watch (2,2)]
		[Field ("HKQuantityTypeIdentifierAppleExerciseTime")]
		AppleExerciseTime,

		[iOS (10,0), Watch (3,0)]
		[Field ("HKQuantityTypeIdentifierDistanceWheelchair")]
		DistanceWheelchair,

		[iOS (10,0), Watch (3,0)]
		[Field ("HKQuantityTypeIdentifierPushCount")]
		PushCount,

		[iOS (10,0), Watch (3,0)]
		[Field ("HKQuantityTypeIdentifierDistanceSwimming")]
		DistanceSwimming,

		[iOS (10,0), Watch (3,0)]
		[Field ("HKQuantityTypeIdentifierSwimmingStrokeCount")]
		SwimmingStrokeCount,

		[Watch (4, 0), iOS (11, 0)]
		[Field ("HKQuantityTypeIdentifierWaistCircumference")]
		WaistCircumference,

		[Watch (4, 0), iOS (11, 0)]
		[Field ("HKQuantityTypeIdentifierVO2Max")]
		VO2Max,

		[Watch (4,2), iOS (11,2)]
		[Field ("HKQuantityTypeIdentifierDistanceDownhillSnowSports")]
		DistanceDownhillSnowSports,

		[iOS (11,0), Watch (4,0)]
		[Field ("HKQuantityTypeIdentifierInsulinDelivery")]
		InsulinDelivery,

		[iOS (11,0), Watch (4,0)]
		[Field ("HKQuantityTypeIdentifierRestingHeartRate")]
		RestingHeartRate,

		[iOS (11,0), Watch (4,0)]
		[Field ("HKQuantityTypeIdentifierWalkingHeartRateAverage")]
		WalkingHeartRateAverage,

		[iOS (11,0), Watch (4,0)]
		[Field ("HKQuantityTypeIdentifierHeartRateVariabilitySDNN")]
		HeartRateVariabilitySdnn,

		[Watch (6, 0), iOS (13, 0)]
		[Field ("HKQuantityTypeIdentifierAppleStandTime")]
		AppleStandTime,

		[Watch (6, 0), iOS (13, 0)]
		[Field ("HKQuantityTypeIdentifierEnvironmentalAudioExposure")]
		EnvironmentalAudioExposure,

		[Watch (6, 0), iOS (13, 0)]
		[Field ("HKQuantityTypeIdentifierHeadphoneAudioExposure")]
		HeadphoneAudioExposure,

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKQuantityTypeIdentifierSixMinuteWalkTestDistance")]
		SixMinuteWalkTestDistance,

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKQuantityTypeIdentifierStairAscentSpeed")]
		StairAscentSpeed,

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKQuantityTypeIdentifierStairDescentSpeed")]
		StairDescentSpeed,

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKQuantityTypeIdentifierWalkingAsymmetryPercentage")]
		WalkingAsymmetryPercentage,

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKQuantityTypeIdentifierWalkingDoubleSupportPercentage")]
		WalkingDoubleSupportPercentage,

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKQuantityTypeIdentifierWalkingSpeed")]
		WalkingSpeed,

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKQuantityTypeIdentifierWalkingStepLength")]
		WalkingStepLength,

		[Watch (7,4)][iOS (14,5)]
		[Field ("HKQuantityTypeIdentifierAppleMoveTime")]
		AppleMoveTime,

		[Watch (8, 0), iOS (15, 0)]
		[Field ("HKQuantityTypeIdentifierAppleWalkingSteadiness")]
		AppleWalkingSteadiness,

		[Watch (8, 0), iOS (15, 0)]
		[Field ("HKQuantityTypeIdentifierNumberOfAlcoholicBeverages")]
		NumberOfAlcoholicBeverages,
	}

	[Watch (2,0)]
	[iOS (8,0)]
	enum HKCorrelationTypeIdentifier {
		[Field ("HKCorrelationTypeIdentifierBloodPressure")]
		BloodPressure,
		
		[Field ("HKCorrelationTypeIdentifierFood")]
		Food,
	}

	[Watch (6, 0), iOS (13, 0)]
	enum HKDataTypeIdentifier
	{
		[Field ("HKDataTypeIdentifierHeartbeatSeries")]
		HeartbeatSeries,
	}

	[Watch (2,0)]
	[iOS (8,0)]
	enum HKCategoryTypeIdentifier
	{
		/**** HKCategoryType Identifiers ****/

		[Field ("HKCategoryTypeIdentifierSleepAnalysis")]
		SleepAnalysis,

		[iOS (9,0)]
		[Field ("HKCategoryTypeIdentifierAppleStandHour")]
		AppleStandHour,

		[iOS (9,0)]
		[Field ("HKCategoryTypeIdentifierCervicalMucusQuality")]
		CervicalMucusQuality,

		[iOS (9,0)]
		[Field ("HKCategoryTypeIdentifierOvulationTestResult")]
		OvulationTestResult,

		[iOS (9,0)]
		[Field ("HKCategoryTypeIdentifierMenstrualFlow")]
		MenstrualFlow,

		[iOS (9,0)]
		[Field ("HKCategoryTypeIdentifierIntermenstrualBleeding")]
		IntermenstrualBleeding,

		[iOS (9,0)]
		[Field ("HKCategoryTypeIdentifierSexualActivity")]
		SexualActivity,

		[iOS (10,0), Watch (3,0)]
		[Field ("HKCategoryTypeIdentifierMindfulSession")]
		MindfulSession,

		[Watch (5,2), iOS (12,2)]
		[Field ("HKCategoryTypeIdentifierHighHeartRateEvent")]
		HighHeartRateEvent,

		[Watch (5,2), iOS (12,2)]
		[Field ("HKCategoryTypeIdentifierLowHeartRateEvent")]
		LowHeartRateEvent,

		[Watch (5,2), iOS (12,2)]
		[Field ("HKCategoryTypeIdentifierIrregularHeartRhythmEvent")]
		IrregularHeartRhythmEvent,

		[Watch (6, 0), iOS (13, 0)]
		[Field ("HKCategoryTypeIdentifierAudioExposureEvent")]
		AudioExposureEvent,

		[Watch (6, 0), iOS (13, 0)]
		[Field ("HKCategoryTypeIdentifierToothbrushingEvent")]
		ToothbrushingEvent,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierAbdominalCramps")]
		AbdominalCramps,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierAcne")]
		Acne,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierAppetiteChanges")]
		AppetiteChanges,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierGeneralizedBodyAche")]
		GeneralizedBodyAche,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierBloating")]
		Bloating,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierBreastPain")]
		BreastPain,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierChestTightnessOrPain")]
		ChestTightnessOrPain,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierChills")]
		Chills,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierConstipation")]
		Constipation,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierCoughing")]
		Coughing,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierDiarrhea")]
		Diarrhea,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierDizziness")]
		Dizziness,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierFainting")]
		Fainting,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierFatigue")]
		Fatigue,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierFever")]
		Fever,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierHeadache")]
		Headache,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierHeartburn")]
		Heartburn,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierHotFlashes")]
		HotFlashes,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierLowerBackPain")]
		LowerBackPain,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierLossOfSmell")]
		LossOfSmell,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierLossOfTaste")]
		LossOfTaste,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierMoodChanges")]
		MoodChanges,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierNausea")]
		Nausea,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierPelvicPain")]
		PelvicPain,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierRapidPoundingOrFlutteringHeartbeat")]
		RapidPoundingOrFlutteringHeartbeat,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierRunnyNose")]
		RunnyNose,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierShortnessOfBreath")]
		ShortnessOfBreath,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierSinusCongestion")]
		SinusCongestion,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierSkippedHeartbeat")]
		SkippedHeartbeat,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierSleepChanges")]
		SleepChanges,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierSoreThroat")]
		SoreThroat,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierVomiting")]
		Vomiting,

		[Watch (7, 0), iOS (13, 6)]
		[Field ("HKCategoryTypeIdentifierWheezing")]
		Wheezing,

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKCategoryTypeIdentifierBladderIncontinence")]
		BladderIncontinence,

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKCategoryTypeIdentifierDrySkin")]
		DrySkin,

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKCategoryTypeIdentifierHairLoss")]
		HairLoss,

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKCategoryTypeIdentifierVaginalDryness")]
		VaginalDryness,

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKCategoryTypeIdentifierMemoryLapse")]
		MemoryLapse,

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKCategoryTypeIdentifierNightSweats")]
		NightSweats,

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKCategoryTypeIdentifierEnvironmentalAudioExposureEvent")]
		EnvironmentalAudioExposureEvent,

		[Watch (7, 0), iOS (14, 0)]
		[Field ("HKCategoryTypeIdentifierHandwashingEvent")]
		HandwashingEvent,

		[Watch (7, 1), iOS (14, 2)]
		[Field ("HKCategoryTypeIdentifierHeadphoneAudioExposureEvent")]
		HeadphoneAudioExposureEvent,

		[Watch (7, 2), iOS (14, 3)]
		[Field ("HKCategoryTypeIdentifierPregnancy")]
		Pregnancy,

		[Watch (7, 2), iOS (14, 3)]
		[Field ("HKCategoryTypeIdentifierLactation")]
		Lactation,

		[Watch (7, 2), iOS (14, 3)]
		[Field ("HKCategoryTypeIdentifierContraceptive")]
		Contraceptive,

		[Watch (7, 2), iOS (14, 3)]
		[Field ("HKCategoryTypeIdentifierLowCardioFitnessEvent")]
		LowCardioFitnessEvent,

		[Watch (8, 0), iOS (15, 0)]
		[Field ("HKCategoryTypeIdentifierAppleWalkingSteadinessEvent")]
		AppleWalkingSteadinessEvent,

		[Watch (8, 0), iOS (15, 0)]
		[Field ("HKCategoryTypeIdentifierPregnancyTestResult")]
		PregnancyTestResult,

		[Watch (8, 0), iOS (15, 0)]
		[Field ("HKCategoryTypeIdentifierProgesteroneTestResult")]
		ProgesteroneTestResult,
	}

	[Watch (2,0)]
	[iOS (8,0)]
	enum HKCharacteristicTypeIdentifier
	{
		/**** HKCharacteristicType Identifiers ****/

		[Field ("HKCharacteristicTypeIdentifierBiologicalSex")]
		BiologicalSex,

		[Field ("HKCharacteristicTypeIdentifierBloodType")]
		BloodType,

		[Field ("HKCharacteristicTypeIdentifierDateOfBirth")]
		DateOfBirth,

		[iOS (9,0)]
		[Field ("HKCharacteristicTypeIdentifierFitzpatrickSkinType")]
		FitzpatrickSkinType,

		[iOS (10,0), Watch (3,0)]
		[Field ("HKCharacteristicTypeIdentifierWheelchairUse")]
		WheelchairUse,

		[Watch (7,0), iOS (14,0)]
		[Field ("HKCharacteristicTypeIdentifierActivityMoveMode")]
		ActivityMoveMode,
	}

	[Watch (2,0)]
	[iOS (8,0)]
	[DisableDefaultCtor] // - (instancetype)init NS_UNAVAILABLE;
	[BaseType (typeof (NSObject))]
	interface HKUnit : NSCopying, NSSecureCoding {

		[Export ("unitString")]
		string UnitString { get; }

		[Static]
		[Export ("unitFromString:")]
		HKUnit FromString (string aString);


		[Static, Export ("unitFromMassFormatterUnit:")]
		HKUnit FromMassFormatterUnit (NSMassFormatterUnit massFormatterUnit);

		[Static, Export ("massFormatterUnitFromUnit:")]
		NSMassFormatterUnit GetMassFormatterUnit (HKUnit unit);

		[Static, Export ("unitFromLengthFormatterUnit:")]
		HKUnit FromLengthFormatterUnit (NSLengthFormatterUnit lengthFormatterUnit);

		[Static, Export ("lengthFormatterUnitFromUnit:")]
		NSLengthFormatterUnit GetLengthFormatterUnit (HKUnit unit);

		[Static, Export ("unitFromEnergyFormatterUnit:")]
		HKUnit FromEnergyFormatterUnit (NSEnergyFormatterUnit energyFormatterUnit);

		[Static, Export ("energyFormatterUnitFromUnit:")]
		NSEnergyFormatterUnit GetEnergyFormatterUnit (HKUnit unit);

		[Export ("isNull")]
		bool IsNull { get; }

		// HKUnit (Mass) Category

		[Static]
		[Export ("gramUnitWithMetricPrefix:")]
		HKUnit FromGramUnit (HKMetricPrefix prefix);

		[Static]
		[Export ("gramUnit")]
		HKUnit Gram { get; }

		[Static]
		[Export ("ounceUnit")]
		HKUnit Ounce { get; }

		[Static]
		[Export ("poundUnit")]
		HKUnit Pound { get; }

		[Static]
		[Export ("stoneUnit")]
		HKUnit Stone { get; }

		[Static]
		[Export ("moleUnitWithMetricPrefix:molarMass:")]
		HKUnit CreateMoleUnit (HKMetricPrefix prefix, double gramsPerMole);

		[Static]
		[Export ("moleUnitWithMolarMass:")]
		HKUnit CreateMoleUnit (double gramsPerMole);

		// HKUnit (Length) Category

		[Static]
		[Export ("meterUnitWithMetricPrefix:")]
		HKUnit CreateMeterUnit (HKMetricPrefix prefix);

		[Static]
		[Export ("meterUnit")]
		HKUnit Meter { get; }

		[Static]
		[Export ("inchUnit")]
		HKUnit Inch { get; }

		[Static]
		[Export ("footUnit")]
		HKUnit Foot { get; }

		[Static]
		[Export ("mileUnit")]
		HKUnit Mile { get; }

		[iOS (9,0)]
		[Static]
		[Export ("yardUnit")]
		HKUnit Yard { get; }

		// HKUnit (Volume) Category

		[Static]
		[Export ("literUnitWithMetricPrefix:")]
		HKUnit CreateLiterUnit (HKMetricPrefix prefix);

		[Static]
		[Export ("literUnit")]
		HKUnit Liter { get; }

		[Static]
		[Export ("fluidOunceUSUnit")]
		HKUnit FluidOunceUSUnit { get; }

		[Static]
		[Export ("fluidOunceImperialUnit")]
		HKUnit FluidOunceImperialUnit { get; }

		[Static]
		[Export ("pintUSUnit")]
		HKUnit PintUSUnit { get; }

		[Static]
		[Export ("pintImperialUnit")]
		HKUnit PintImperialUnit { get; }

		[Static]
		[iOS (9,0)]
		[Export ("cupUSUnit")]
		HKUnit CupUSUnit { get; }

		[Static]
		[iOS (9,0)]
		[Export ("cupImperialUnit")]
		HKUnit CupImperialUnit { get; }

		// HKUnit (Pressure) Category

		[Static]
		[Export ("pascalUnitWithMetricPrefix:")]
		HKUnit CreatePascalUnit (HKMetricPrefix prefix);

		[Static]
		[Export ("pascalUnit")]
		HKUnit Pascal { get; }

		[Static]
		[Export ("millimeterOfMercuryUnit")]
		HKUnit MillimeterOfMercury { get; }

		[Static]
		[Export ("centimeterOfWaterUnit")]
		HKUnit CentimeterOfWater { get; }

		[Static]
		[Export ("atmosphereUnit")]
		HKUnit Atmosphere { get; }

		[Watch (7,0), iOS (14,0)]
		[Static]
		[Export ("inchesOfMercuryUnit")]
		HKUnit InchesOfMercury { get; }

		// HKUnit (Time) Category

		[Static]
		[Export ("secondUnitWithMetricPrefix:")]
		HKUnit CreateSecondUnit (HKMetricPrefix prefix);

		[Static]
		[Export ("secondUnit")]
		HKUnit Second { get; }

		[Static]
		[Export ("minuteUnit")]
		HKUnit Minute { get; }

		[Static]
		[Export ("hourUnit")]
		HKUnit Hour { get; }

		[Static]
		[Export ("dayUnit")]
		HKUnit Day { get; }

		// HKUnit (Energy) Category

		[Static]
		[Export ("jouleUnitWithMetricPrefix:")]
		HKUnit CreateJouleUnit (HKMetricPrefix prefix);

		[Static]
		[Export ("jouleUnit")]
		HKUnit Joule { get; }

		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'SmallCalorie' or 'LargeCalorie' instead.")]
		[Deprecated (PlatformName.WatchOS, 4, 0, message: "Use 'SmallCalorie' or 'LargeCalorie' instead.")]
		[Static]
		[Export ("calorieUnit")]
		HKUnit Calorie { get; }

		[Static]
		[Export ("kilocalorieUnit")]
		HKUnit Kilocalorie { get; }

		[Watch (4, 0), iOS (11, 0)]
		[Static]
		[Export ("smallCalorieUnit")]
		HKUnit SmallCalorie { get; }

		[Watch (4, 0), iOS (11, 0)]
		[Static]
		[Export ("largeCalorieUnit")]
		HKUnit LargeCalorie { get; }

		// HKUnit (Temperature) Category

		[Static]
		[Export ("degreeCelsiusUnit")]
		HKUnit DegreeCelsius { get; }

		[Static]
		[Export ("degreeFahrenheitUnit")]
		HKUnit DegreeFahrenheit { get; }

		[Static]
		[Export ("kelvinUnit")]
		HKUnit Kelvin { get; }

		// HKUnit(Conductance) Category

		[Static]
		[Export ("siemenUnitWithMetricPrefix:")]
		HKUnit CreateSiemenUnit (HKMetricPrefix prefix);

		[Static]
		[Export ("siemenUnit")]
		HKUnit Siemen { get; }

		// HKUnit (Scalar) Category

		[Static]
		[Export ("countUnit")]
		HKUnit Count { get; }

		[Static]
		[Export ("percentUnit")]
		HKUnit Percent { get; }

		// HKUnit (Math) Category

		[Export ("unitMultipliedByUnit:")]
		HKUnit UnitMultipliedBy (HKUnit unit);

		[Export ("unitDividedByUnit:")]
		HKUnit UnitDividedBy (HKUnit unit);

		[Export ("unitRaisedToPower:")]
		HKUnit UnitRaisedToPower (nint power);

		[Export ("reciprocalUnit")]
		HKUnit ReciprocalUnit ();

		// HKUnit (Pharmacology) Category
		[Watch (4, 0), iOS (11, 0)]
		[Static]
		[Export ("internationalUnit")]
		HKUnit InternationalUnit { get; }

		// HKUnit (DecibelAWeightedSoundPressureLevel) Category
		[Watch (6,0), iOS (13,0)]
		[Static]
		[Export ("decibelAWeightedSoundPressureLevelUnit")]
		HKUnit DecibelAWeightedSoundPressureLevelUnit { get; }

		// HKUnit (HearingSensitivity) Category
		[Watch (6,0), iOS (13,0)]
		[Static]
		[Export ("decibelHearingLevelUnit")]
		HKUnit DecibelHearingLevelUnit { get; }

		// HKUnit (Frequency) Category

		[Watch (6,0), iOS (13,0)]
		[Static]
		[Export ("hertzUnitWithMetricPrefix:")]
		HKUnit GetHertzUnit (HKMetricPrefix prefix);

		[Watch (6,0), iOS (13,0)]
		[Static]
		[Export ("hertzUnit")]
		HKUnit HertzUnit { get; }

		// HKUnit (ElectricPotentialDifference) Category

		[Watch (7,0), iOS (14,0)]
		[Static]
		[Export ("voltUnitWithMetricPrefix:")]
		HKUnit GetVolt (HKMetricPrefix prefix);

		[Watch (7,0), iOS (14,0)]
		[Static]
		[Export ("voltUnit")]
		HKUnit Volt { get; }
	}

	[Watch (2,0)]
	[iOS (8,0)]
	[BaseType (typeof (HKSample))]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKWorkout
	interface HKWorkout {
		[Export ("workoutActivityType")]
		HKWorkoutActivityType WorkoutActivityType { get; }

		[Export ("workoutEvents")]
		HKWorkoutEvent [] WorkoutEvents { get; }

		[Export ("duration", ArgumentSemantic.UnsafeUnretained)]
		double Duration { get; }

		[Export ("totalEnergyBurned", ArgumentSemantic.Retain)]
		HKQuantity TotalEnergyBurned { get; }

		[Export ("totalDistance", ArgumentSemantic.Retain)]
		HKQuantity TotalDistance { get; }

		[Watch (3,0), iOS (10,0)]
		[NullAllowed, Export ("totalSwimmingStrokeCount", ArgumentSemantic.Strong)]
		HKQuantity TotalSwimmingStrokeCount { get; }

		[Static, Export ("workoutWithActivityType:startDate:endDate:")]
		HKWorkout Create (HKWorkoutActivityType workoutActivityType, NSDate startDate, NSDate endDate);

		[Static, Export ("workoutWithActivityType:startDate:endDate:workoutEvents:totalEnergyBurned:totalDistance:metadata:")]
		[EditorBrowsable (EditorBrowsableState.Advanced)] // this is not the one we want to be seen (compat only)
		HKWorkout Create (HKWorkoutActivityType workoutActivityType, NSDate startDate, NSDate endDate, [NullAllowed] HKWorkoutEvent [] workoutEvents, [NullAllowed] HKQuantity totalEnergyBurned, [NullAllowed] HKQuantity totalDistance, [NullAllowed] NSDictionary metadata);

		[Static, Wrap ("Create (workoutActivityType, startDate, endDate, workoutEvents, totalEnergyBurned, totalDistance, metadata.GetDictionary ())")]
		HKWorkout Create (HKWorkoutActivityType workoutActivityType, NSDate startDate, NSDate endDate, HKWorkoutEvent [] workoutEvents, HKQuantity totalEnergyBurned, HKQuantity totalDistance, HKMetadata metadata);
		
		[Static, Export ("workoutWithActivityType:startDate:endDate:duration:totalEnergyBurned:totalDistance:metadata:")]
		[EditorBrowsable (EditorBrowsableState.Advanced)] // this is not the one we want to be seen (compat only)
		HKWorkout Create (HKWorkoutActivityType workoutActivityType, NSDate startDate, NSDate endDate, double duration, [NullAllowed] HKQuantity totalEnergyBurned, [NullAllowed] HKQuantity totalDistance, [NullAllowed] NSDictionary metadata);

		[Static, Wrap ("Create (workoutActivityType, startDate, endDate, duration, totalEnergyBurned, totalDistance, metadata.GetDictionary ())")]
		HKWorkout Create (HKWorkoutActivityType workoutActivityType, NSDate startDate, NSDate endDate, double duration, HKQuantity totalEnergyBurned, HKQuantity totalDistance, HKMetadata metadata);

		[iOS (9,0)]
		[Static]
		[Export ("workoutWithActivityType:startDate:endDate:workoutEvents:totalEnergyBurned:totalDistance:device:metadata:")]
		HKWorkout Create (HKWorkoutActivityType workoutActivityType, NSDate startDate, NSDate endDate, [NullAllowed] HKWorkoutEvent[] workoutEvents, [NullAllowed] HKQuantity totalEnergyBurned, [NullAllowed] HKQuantity totalDistance, [NullAllowed] HKDevice device, [NullAllowed] NSDictionary metadata);

		[iOS (9,0)]
		[Static]
		[Wrap ("Create (workoutActivityType, startDate, endDate, workoutEvents, totalEnergyBurned, totalDistance, device, metadata.GetDictionary ())")]
		HKWorkout Create (HKWorkoutActivityType workoutActivityType, NSDate startDate, NSDate endDate, HKWorkoutEvent[] workoutEvents, HKQuantity totalEnergyBurned, HKQuantity totalDistance, HKDevice device, HKMetadata metadata);

		[iOS (9,0)]
		[Static]
		[Export ("workoutWithActivityType:startDate:endDate:duration:totalEnergyBurned:totalDistance:device:metadata:")]
		HKWorkout Create (HKWorkoutActivityType workoutActivityType, NSDate startDate, NSDate endDate, double duration, [NullAllowed] HKQuantity totalEnergyBurned, [NullAllowed] HKQuantity totalDistance, [NullAllowed] HKDevice device, [NullAllowed] NSDictionary metadata);

		[iOS (9,0)]
		[Static]
		[Wrap ("Create (workoutActivityType, startDate, endDate, duration, totalEnergyBurned, totalDistance, device, metadata.GetDictionary ())")]
		HKWorkout Create (HKWorkoutActivityType workoutActivityType, NSDate startDate, NSDate endDate, double duration, HKQuantity totalEnergyBurned, HKQuantity totalDistance, HKDevice device, HKMetadata metadata);

		[Watch (3,0), iOS (10,0)]
		[Static]
		[Export ("workoutWithActivityType:startDate:endDate:workoutEvents:totalEnergyBurned:totalDistance:totalSwimmingStrokeCount:device:metadata:")]
		HKWorkout Create (HKWorkoutActivityType workoutActivityType, NSDate startDate, NSDate endDate, [NullAllowed] HKWorkoutEvent[] workoutEvents, [NullAllowed] HKQuantity totalEnergyBurned, [NullAllowed] HKQuantity totalDistance, [NullAllowed] HKQuantity totalSwimmingStrokeCount, [NullAllowed] HKDevice device, [NullAllowed] NSDictionary metadata);

		[Watch (3,0), iOS (10,0)]
		[Static]
		[Wrap ("Create (workoutActivityType, startDate, endDate, workoutEvents, totalEnergyBurned, totalDistance, totalSwimmingStrokeCount, device, metadata.GetDictionary ())")]
		HKWorkout Create (HKWorkoutActivityType workoutActivityType, NSDate startDate, NSDate endDate, HKWorkoutEvent[] workoutEvents, HKQuantity totalEnergyBurned, HKQuantity totalDistance, HKQuantity totalSwimmingStrokeCount, HKDevice device, HKMetadata metadata);

		[Watch (4, 0), iOS (11, 0)]
		[Static]
		[Export ("workoutWithActivityType:startDate:endDate:workoutEvents:totalEnergyBurned:totalDistance:totalFlightsClimbed:device:metadata:")]
		HKWorkout CreateFlightsClimbedWorkout (HKWorkoutActivityType workoutActivityType, NSDate startDate, NSDate endDate, [NullAllowed] HKWorkoutEvent [] workoutEvents, [NullAllowed] HKQuantity totalEnergyBurned, [NullAllowed] HKQuantity totalDistance, [NullAllowed] HKQuantity totalFlightsClimbed, [NullAllowed] HKDevice device, [NullAllowed] NSDictionary metadata);

		[Watch (4, 0), iOS (11, 0)]
		[Static]
		[Wrap ("CreateFlightsClimbedWorkout (workoutActivityType, startDate, endDate, workoutEvents, totalEnergyBurned, totalDistance, totalFlightsClimbed, device, metadata.GetDictionary ())")]
		HKWorkout CreateFlightsClimbedWorkout (HKWorkoutActivityType workoutActivityType, NSDate startDate, NSDate endDate, [NullAllowed] HKWorkoutEvent [] workoutEvents, [NullAllowed] HKQuantity totalEnergyBurned, [NullAllowed] HKQuantity totalDistance, [NullAllowed] HKQuantity totalFlightsClimbed, [NullAllowed] HKDevice device, [NullAllowed] HKMetadata metadata);

		// TODO: where is this thing used?
		[Field ("HKWorkoutSortIdentifierDuration")]
		NSString SortIdentifierDuration { get; }

		// TODO: where is this thing used?
		[Field ("HKWorkoutSortIdentifierTotalDistance")]
		NSString SortIdentifierTotalDistance { get; }

		// TODO: where is this thing used?
		[Field ("HKWorkoutSortIdentifierTotalEnergyBurned")]
		NSString SortIdentifierTotalEnergyBurned { get; }

		[Watch (3,0), iOS (10,0)]
		[Field ("HKWorkoutSortIdentifierTotalSwimmingStrokeCount")]
		NSString SortIdentifierTotalSwimmingStrokeCount { get; }

		[Watch (4, 0), iOS (11, 0)]
		[Field ("HKWorkoutSortIdentifierTotalFlightsClimbed")]
		NSString SortIdentifierTotalFlightsClimbed { get; }

		[Watch (4, 0), iOS (11, 0)]
		[NullAllowed, Export ("totalFlightsClimbed", ArgumentSemantic.Strong)]
		HKQuantity TotalFlightsClimbed { get; }
	}

	[Watch (2,0)]
	[iOS (8,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface HKWorkoutEvent : NSSecureCoding, NSCopying {
		[Export ("type")]
		HKWorkoutEventType Type { get; }

		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'DateInterval' instead.")]
		[Deprecated (PlatformName.WatchOS, 4, 0, message: "Use 'DateInterval' instead.")]
		[Export ("date", ArgumentSemantic.Copy)]
		NSDate Date { get; }

		[Watch (3,0), iOS (10,0)]
		[NullAllowed, Export ("metadata", ArgumentSemantic.Copy)]
		NSDictionary WeakMetadata { get; }

		[Watch (3,0), iOS (10,0)]
		[Wrap ("WeakMetadata")]
		HKMetadata Metadata { get; }

		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'Create (HKWorkoutEventType, NSDateInterval, HKMetadata)' instead.")]
		[Deprecated (PlatformName.WatchOS, 4, 0, message: "Use 'Create (HKWorkoutEventType, NSDateInterval, HKMetadata)' instead.")]
		[Static, Export ("workoutEventWithType:date:")]
		HKWorkoutEvent Create (HKWorkoutEventType type, NSDate date);

		[Watch (3,0), iOS (10,0)]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'Create (HKWorkoutEventType, NSDateInterval, HKMetadata)' instead.")]
		[Deprecated (PlatformName.WatchOS, 4, 0, message: "Use 'Create (HKWorkoutEventType, NSDateInterval, HKMetadata)' instead.")]
		[Static]
		[EditorBrowsable (EditorBrowsableState.Advanced)] // this is not the one we want to be seen (compat only)
		[Export ("workoutEventWithType:date:metadata:")]
		HKWorkoutEvent Create (HKWorkoutEventType type, NSDate date, NSDictionary metadata);

		[Watch (3,0), iOS (10,0)]
		[Deprecated (PlatformName.iOS, 11, 0, message: "Use 'Create (HKWorkoutEventType, NSDateInterval, HKMetadata)' instead.")]
		[Deprecated (PlatformName.WatchOS, 4, 0, message: "Use 'Create (HKWorkoutEventType, NSDateInterval, HKMetadata)' instead.")]
		[Static]
		[Wrap ("Create (type, date, metadata.GetDictionary ()!)")]
		HKWorkoutEvent Create (HKWorkoutEventType type, NSDate date, HKMetadata metadata);

		[Watch (4, 0), iOS (11, 0)]
		[Static]
		[Export ("workoutEventWithType:dateInterval:metadata:")]
		HKWorkoutEvent Create (HKWorkoutEventType type, NSDateInterval dateInterval, [NullAllowed] NSDictionary metadata);

		[Watch (4, 0), iOS (11, 0)]
		[Static]
		[Wrap ("Create (type, dateInterval, metadata.GetDictionary ())")]
		HKWorkoutEvent Create (HKWorkoutEventType type, NSDateInterval dateInterval, HKMetadata metadata);

		[Watch (4, 0), iOS (11, 0)]
		[Export ("dateInterval", ArgumentSemantic.Copy)]
		NSDateInterval DateInterval { get; }
	}

	[Watch (2,0)]
	[iOS (8,0)]
	[BaseType (typeof (HKSampleType))]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKWorkoutType
	interface HKWorkoutType {
		[Field ("HKWorkoutTypeIdentifier")]
		NSString Identifier { get; }
	}

	[Watch (2,0)]
	[iOS (9,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface HKDeletedObject : NSSecureCoding {
		[Export ("UUID", ArgumentSemantic.Strong)]
		NSUuid Uuid { get; }

		[Watch (4, 0), iOS (11, 0)]
		[NullAllowed, Export ("metadata", ArgumentSemantic.Copy)]
		NSDictionary WeakMetadata { get; }

		[Watch (4, 0), iOS (11, 0)]
		[Wrap ("WeakMetadata")]
		HKMetadata Metadata { get; }
	}

	[Watch (2,0)]
	[iOS (9,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface HKDevice : NSSecureCoding, NSCopying {
		[NullAllowed]
		[Export ("name")]
		string Name { get; }

		[NullAllowed, Export ("manufacturer")]
		string Manufacturer { get; }

		[NullAllowed, Export ("model")]
		string Model { get; }

		[NullAllowed, Export ("hardwareVersion")]
		string HardwareVersion { get; }

		[NullAllowed, Export ("firmwareVersion")]
		string FirmwareVersion { get; }

		[NullAllowed, Export ("softwareVersion")]
		string SoftwareVersion { get; }

		[NullAllowed, Export ("localIdentifier")]
		string LocalIdentifier { get; }

		[NullAllowed, Export ("UDIDeviceIdentifier")]
		string UdiDeviceIdentifier { get; }

		[Export ("initWithName:manufacturer:model:hardwareVersion:firmwareVersion:softwareVersion:localIdentifier:UDIDeviceIdentifier:")]
		NativeHandle Constructor ([NullAllowed] string name, [NullAllowed] string manufacturer, [NullAllowed] string model, [NullAllowed] string hardwareVersion, [NullAllowed] string firmwareVersion, [NullAllowed] string softwareVersion, [NullAllowed] string localIdentifier, [NullAllowed] string udiDeviceIdentifier);

		[Static]
		[Export ("localDevice")]
		HKDevice LocalDevice { get; }
	}

	[NoWatch, iOS (10,0)]
	[BaseType (typeof(HKQuery))]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKDocumentQuery
	interface HKDocumentQuery
	{
		[Export ("limit")]
		nuint Limit { get; }

		[NullAllowed, Export ("sortDescriptors", ArgumentSemantic.Copy)]
		NSSortDescriptor[] SortDescriptors { get; }

		[Export ("includeDocumentData")]
		bool IncludeDocumentData { get; }

		[Export ("initWithDocumentType:predicate:limit:sortDescriptors:includeDocumentData:resultsHandler:")]
		NativeHandle Constructor (HKDocumentType documentType, [NullAllowed] NSPredicate predicate, nuint limit, [NullAllowed] NSSortDescriptor[] sortDescriptors, bool includeDocumentData, Action<HKDocumentQuery, HKDocumentSample [], bool, NSError> resultsHandler);
	}

	[Watch (2,0)]
	[iOS (9,0)]
	[Static]
	interface HKDevicePropertyKey {
		[Field ("HKDevicePropertyKeyName")]
		NSString Name { get; }

		[Field ("HKDevicePropertyKeyManufacturer")]
		NSString Manufacturer { get; }

		[Field ("HKDevicePropertyKeyModel")]
		NSString Model { get; }

		[Field ("HKDevicePropertyKeyHardwareVersion")]
		NSString HardwareVersion { get; }

		[Field ("HKDevicePropertyKeyFirmwareVersion")]
		NSString FirmwareVersion { get; }

		[Field ("HKDevicePropertyKeySoftwareVersion")]
		NSString SoftwareVersion { get; }

		[Field ("HKDevicePropertyKeyLocalIdentifier")]
		NSString LocalIdentifier { get; }

		[Field ("HKDevicePropertyKeyUDIDeviceIdentifier")]
		NSString UdiDeviceIdentifier { get; }
	}

	[Watch (2,0)]
	[iOS (9,0)]
	[BaseType (typeof (NSObject))]
	interface HKFitzpatrickSkinTypeObject : NSCopying, NSSecureCoding {
		[Export ("skinType")]
		HKFitzpatrickSkinType SkinType { get; }
	}

	[Watch (3,0), iOS (10,0)]
	[BaseType (typeof(NSObject))]
	interface HKWheelchairUseObject : NSCopying, NSSecureCoding {
		[Export ("wheelchairUse")]
		HKWheelchairUse WheelchairUse { get; }
	}

	[Watch (2,0)]
	[iOS (9,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface HKSourceRevision : NSSecureCoding, NSCopying {
		[Export ("source")]
		HKSource Source { get; }

		[NullAllowed, Export ("version")]
		string Version { get; }

		[Export ("initWithSource:version:")]
		NativeHandle Constructor (HKSource source, [NullAllowed] string version);

		[Watch (4, 0), iOS (11, 0)]
		[NullAllowed, Export ("productType")]
		string ProductType { get; }

		[Watch (4, 0), iOS (11, 0)]
		[Export ("operatingSystemVersion", ArgumentSemantic.Assign)]
		NSOperatingSystemVersion OperatingSystemVersion { get; }

		[Watch (4, 0), iOS (11, 0)]
		[Export ("initWithSource:version:productType:operatingSystemVersion:")]
		NativeHandle Constructor (HKSource source, [NullAllowed] string version, [NullAllowed] string productType, NSOperatingSystemVersion operatingSystemVersion);
	}

	[Watch (4,0), iOS (11,0)]
	[Static]
	interface HKSourceRevisionInfo {

		[Field ("HKSourceRevisionAnyVersion")]
		NSString AnyVersion { get; }

		[Field ("HKSourceRevisionAnyProductType")]
		NSString AnyProductType { get; }

		// This key seems broken even in Objc returns a weird value
		//[Internal]
		//[Field ("HKSourceRevisionAnyOperatingSystem")]
		//IntPtr _AnyOperatingSystem { get; }

		//[Static]
		//[Wrap ("System.Runtime.InteropServices.Marshal.PtrToStructure<NSOperatingSystemVersion> (_AnyOperatingSystem)")]
		//NSOperatingSystemVersion AnyOperatingSystem { get; }
	}

	[Watch (2,0)]
	[iOS (9,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface HKQueryAnchor : NSSecureCoding, NSCopying {
		[Static]
		[Export ("anchorFromValue:")]
		HKQueryAnchor Create (nuint value);
	}


	[NoiOS]
	[Watch (2,0)]
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface HKWorkoutSession : NSSecureCoding {
		[Deprecated (PlatformName.WatchOS, 3, 0, message: "Use 'WorkoutConfiguration' instead.")]
		[Export ("activityType")]
		HKWorkoutActivityType ActivityType { get; }

		[Deprecated (PlatformName.WatchOS, 3, 0, message: "Use 'WorkoutConfiguration' instead.")]
		[Export ("locationType")]
		HKWorkoutSessionLocationType LocationType { get; }

		[Watch (3,0)]
		[Export ("workoutConfiguration", ArgumentSemantic.Copy)]
		HKWorkoutConfiguration WorkoutConfiguration { get; }

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		[Protocolize]
		HKWorkoutSessionDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Export ("state")]
		HKWorkoutSessionState State { get; }

		[NullAllowed, Export ("startDate")]
		NSDate StartDate { get; }

		[NullAllowed, Export ("endDate")]
		NSDate EndDate { get; }

		[Deprecated (PlatformName.WatchOS, 3, 0, message: "Use HKWorkoutSession (HKHealthStore, HKWorkoutConfiguration, out NSError) instead.")]
		[Export ("initWithActivityType:locationType:")]
		NativeHandle Constructor (HKWorkoutActivityType activityType, HKWorkoutSessionLocationType locationType);

		[Watch (3,0)]
		[Deprecated (PlatformName.WatchOS, 5, 0, message: "Use HKWorkoutSession (HKHealthStore, HKWorkoutConfiguration, out NSError) instead.")]
		[Export ("initWithConfiguration:error:")]
		NativeHandle Constructor (HKWorkoutConfiguration workoutConfiguration, out NSError error);

		[Watch (5,0)]
		[Export ("initWithHealthStore:configuration:error:")]
		NativeHandle Constructor (HKHealthStore healthStore, HKWorkoutConfiguration workoutConfiguration, [NullAllowed] out NSError error);

		[Watch (5,0)]
		[Export ("prepare")]
		void Prepare ();

		[Watch (5,0)]
		[Export ("startActivityWithDate:")]
		void StartActivity ([NullAllowed] NSDate date);

		[Watch (5,0)]
		[Export ("stopActivityWithDate:")]
		void StopActivity ([NullAllowed] NSDate date);

		[Watch (5,0)]
		[Export ("end")]
		void End ();

		[Watch (5,0)]
		[Export ("pause")]
		void Pause ();

		[Watch (5,0)]
		[Export ("resume")]
		void Resume ();

		[Watch (5,0)]
		[Export ("associatedWorkoutBuilder")]
		HKLiveWorkoutBuilder AssociatedWorkoutBuilder { get; }
	}

	[NoiOS]
	[Watch (2,0)]
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface HKWorkoutSessionDelegate {
		[Abstract]
		[Export ("workoutSession:didChangeToState:fromState:date:")]
		void DidChangeToState (HKWorkoutSession workoutSession, HKWorkoutSessionState toState, HKWorkoutSessionState fromState, NSDate date);

		[Abstract]
		[Export ("workoutSession:didFailWithError:")]
		void DidFail (HKWorkoutSession workoutSession, NSError error);

		[Watch (3,0), iOS (10,0)]
		[Export ("workoutSession:didGenerateEvent:")]
		void DidGenerateEvent (HKWorkoutSession workoutSession, HKWorkoutEvent @event);
	}

	[iOS (9,3), Watch (2,2)]
	[BaseType (typeof (NSObject))]
	interface HKActivitySummary : NSSecureCoding, NSCopying {
		[Export ("dateComponentsForCalendar:")]
		NSDateComponents DateComponentsForCalendar (NSCalendar calendar);

		[Watch (7, 0), iOS (14, 0)]
		[Export ("activityMoveMode", ArgumentSemantic.Assign)]
		HKActivityMoveMode ActivityMoveMode { get; set; }

		[Export ("activeEnergyBurned", ArgumentSemantic.Strong)]
		HKQuantity ActiveEnergyBurned { get; set; }

		[Watch (7, 0), iOS (14, 0)]
		[Export ("appleMoveTime", ArgumentSemantic.Strong)]
		HKQuantity AppleMoveTime { get; set; }

		[Export ("appleExerciseTime", ArgumentSemantic.Strong)]
		HKQuantity AppleExerciseTime { get; set; }

		[Export ("appleStandHours", ArgumentSemantic.Strong)]
		HKQuantity AppleStandHours { get; set; }

		[Export ("activeEnergyBurnedGoal", ArgumentSemantic.Strong)]
		HKQuantity ActiveEnergyBurnedGoal { get; set; }

		[Watch (7, 0), iOS (14, 0)]
		[Export ("appleMoveTimeGoal", ArgumentSemantic.Strong)]
		HKQuantity AppleMoveTimeGoal { get; set; }

		[Export ("appleExerciseTimeGoal", ArgumentSemantic.Strong)]
		HKQuantity AppleExerciseTimeGoal { get; set; }

		[Export ("appleStandHoursGoal", ArgumentSemantic.Strong)]
		HKQuantity AppleStandHoursGoal { get; set; }
	}

	[iOS (9,3), Watch (2,2)]
	[BaseType (typeof (HKQuery))]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKActivitySummaryQuery
	interface HKActivitySummaryQuery {
		[NullAllowed, Export ("updateHandler", ArgumentSemantic.Copy)]
		Action<HKActivitySummaryQuery, HKActivitySummary[], NSError> UpdateHandler { get; set; }

		[Export ("initWithPredicate:resultsHandler:")]
		NativeHandle Constructor ([NullAllowed] NSPredicate predicate, Action<HKActivitySummaryQuery, HKActivitySummary[], NSError> handler);
	}

	[iOS (9,3), Watch (2,2)]
	[BaseType (typeof (HKObjectType))]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKActivitySummaryType
	interface HKActivitySummaryType {
	}

	[Watch (3,0)][iOS (10,0)]
	[BaseType (typeof (NSObject))]
	interface HKWorkoutConfiguration : NSCopying, NSSecureCoding {

		[Export ("activityType", ArgumentSemantic.Assign)]
		HKWorkoutActivityType ActivityType { get; set; }

		[Export ("locationType", ArgumentSemantic.Assign)]
		HKWorkoutSessionLocationType LocationType { get; set; }

		[Export ("swimmingLocationType", ArgumentSemantic.Assign)]
		HKWorkoutSwimmingLocationType SwimmingLocationType { get; set; }

		[NullAllowed, Export ("lapLength", ArgumentSemantic.Copy)]
		HKQuantity LapLength { get; set; }
	}

	[Watch (4, 0), iOS (11, 0)]
	[BaseType (typeof (HKSampleType))]
	[DisableDefaultCtor]
	interface HKSeriesType {
		[Static]
		[Export ("workoutRouteType")]
		HKSeriesType WorkoutRouteType { get; }

		[Watch (6,0), iOS (13,0)]
		[Static]
		[Export ("heartbeatSeriesType")]
		HKSeriesType HeartbeatSeriesType { get; }
	}

	[iOS (11,0)]
	[Watch (4,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface HKSeriesBuilder : NSSecureCoding {
		[Export ("discard")]
		void Discard ();
	}

	[iOS (11,0)]
	[Watch (4,0)]
	[BaseType (typeof(HKSample))]
	[DisableDefaultCtor]
	interface HKSeriesSample : NSCopying {
		[Export ("count")]
		nuint Count { get; }
	}

	[Watch (4, 0), iOS (11, 0)]
	[BaseType (typeof(HKSeriesSample))]
	[DisableDefaultCtor]
	interface HKWorkoutRoute : NSCopying {

		[Field ("HKWorkoutRouteTypeIdentifier")]
		NSString TypeIdentifier { get; }
	}

	delegate void HKWorkoutRouteBuilderAddMetadataHandler (bool success, NSError error);
	[Watch (4, 0), iOS (11, 0)]
	[BaseType (typeof (HKSeriesBuilder))]
	[DisableDefaultCtor]
	interface HKWorkoutRouteBuilder {
		[Export ("initWithHealthStore:device:")]
		NativeHandle Constructor (HKHealthStore healthStore, [NullAllowed] HKDevice device);

		[Async, Export ("insertRouteData:completion:")]
		void InsertRouteData (CLLocation [] routeData, Action<bool, NSError> completion);

		[Async, Protected, Export ("finishRouteWithWorkout:metadata:completion:")]
		void FinishRoute (HKWorkout workout, [NullAllowed] NSDictionary metadata, Action<HKWorkoutRoute, NSError> completion);

		[Async, Wrap ("FinishRoute (workout, metadata.GetDictionary (), completion)")]
		void FinishRoute (HKWorkout workout, HKMetadata metadata, Action<HKWorkoutRoute, NSError> completion);

		[Watch (5, 0), iOS (12, 0)]
		[Async, Protected]
		[Export ("addMetadata:completion:")]
		void AddMetadata (NSDictionary metadata, HKWorkoutRouteBuilderAddMetadataHandler completion);

		[Watch (5, 0), iOS (12, 0)]
		[Async, Wrap ("AddMetadata (metadata.GetDictionary ()!, completion)")]
		void AddMetadata (HKMetadata metadata, HKWorkoutRouteBuilderAddMetadataHandler completion);
	}

	[Watch (4,0), iOS (11,0)]
	[BaseType (typeof(HKQuery))]
	interface HKWorkoutRouteQuery {
		[Export ("initWithRoute:dataHandler:")]
		[DesignatedInitializer]
		NativeHandle Constructor (HKWorkoutRoute workoutRoute, HKWorkoutRouteBuilderDataHandler dataHandler);
	}

	delegate void HKWorkoutBuilderCompletionHandler (bool success, NSError error);
	[Watch (5,0), iOS (12,0)]
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface HKWorkoutBuilder
	{
		[NullAllowed, Export ("device", ArgumentSemantic.Copy)]
		HKDevice Device { get; }

		[NullAllowed, Export ("startDate", ArgumentSemantic.Copy)]
		NSDate StartDate { get; }

		[NullAllowed, Export ("endDate", ArgumentSemantic.Copy)]
		NSDate EndDate { get; }

		[Export ("workoutConfiguration", ArgumentSemantic.Copy)]
		HKWorkoutConfiguration WorkoutConfiguration { get; }

		[Protected]
		[Export ("metadata", ArgumentSemantic.Copy)]
		NSDictionary NativeMetadata { get; }

		[Wrap ("NativeMetadata")]
		HKMetadata Metadata { get; }

		[Export ("workoutEvents", ArgumentSemantic.Copy)]
		HKWorkoutEvent[] WorkoutEvents { get; }

		[Export ("initWithHealthStore:configuration:device:")]
		NativeHandle Constructor (HKHealthStore healthStore, HKWorkoutConfiguration configuration, [NullAllowed] HKDevice device);

		[Async]
		[Export ("beginCollectionWithStartDate:completion:")]
		void BeginCollection (NSDate startDate, HKWorkoutBuilderCompletionHandler completionHandler);

		[Async]
		[Export ("addSamples:completion:")]
		void Add (HKSample[] samples, HKWorkoutBuilderCompletionHandler completionHandler);

		[Async]
		[Export ("addWorkoutEvents:completion:")]
		void Add (HKWorkoutEvent[] workoutEvents, HKWorkoutBuilderCompletionHandler completionHandler);

		[Async, Protected]
		[Export ("addMetadata:completion:")]
		void Add (NSDictionary metadata, HKWorkoutBuilderCompletionHandler completionHandler);

		[Async]
		[Wrap ("Add (metadata.GetDictionary ()!, completionHandler)")]
		void Add (HKMetadata metadata, HKWorkoutBuilderCompletionHandler completionHandler);

		[Async]
		[Export ("endCollectionWithEndDate:completion:")]
		void EndCollection (NSDate endDate, HKWorkoutBuilderCompletionHandler completionHandler);

		[Async]
		[Export ("finishWorkoutWithCompletion:")]
		void FinishWorkout (HKWorkoutBuilderCompletionHandler completionHandler);

		[Export ("discardWorkout")]
		void DiscardWorkout ();

		[Export ("elapsedTimeAtDate:")]
		double GetElapsedTime (NSDate date);

		[Export ("statisticsForType:")]
		[return: NullAllowed]
		HKStatistics GetStatistics (HKQuantityType quantityType);

		[Export ("seriesBuilderForType:")]
		HKSeriesBuilder GetSeriesBuilder (HKSeriesType seriesType);
	}

	delegate void HKQuantitySeriesSampleQueryQuantityDelegate (HKQuantitySeriesSampleQuery query, HKQuantity quantity, NSDate date, bool done, NSError error);
	delegate void HKQuantitySeriesSampleQueryQuantityHandler (HKQuantitySeriesSampleQuery query, HKQuantity quantity, NSDateInterval date, bool done, NSError error);

	[Watch (5,0), iOS (12,0)]
	[BaseType (typeof(HKQuery))]
	interface HKQuantitySeriesSampleQuery
	{
		[Watch (6, 0), iOS (13, 0)]
		[Export ("includeSample")]
		bool IncludeSample { get; set; }

		[Watch (6, 0), iOS (13, 0)]
		[Export ("orderByQuantitySampleStartDate")]
		bool OrderByQuantitySampleStartDate { get; set; }

		[Watch (6,0), iOS (13,0)]
		[Export ("initWithQuantityType:predicate:quantityHandler:")]
		NativeHandle Constructor (HKQuantityType quantityType, [NullAllowed] NSPredicate predicate, HKQuantitySeriesSampleQueryQuantityHandler quantityHandler);

		[Deprecated (PlatformName.iOS, 13, 0, message: "Use Constructor that takes 'NSDateInterval' instead.")]
		[Deprecated (PlatformName.WatchOS, 6, 0, message: "Use Constructor that takes 'NSDateInterval' instead.")]
		[Export ("initWithSample:quantityHandler:")]
		NativeHandle Constructor (HKQuantitySample quantitySample, HKQuantitySeriesSampleQueryQuantityDelegate quantityHandler);
	}

	delegate void HKQuantitySeriesSampleBuilderFinishSeriesDelegate (HKQuantitySample [] samples, NSError error);

	[Watch (5,0), iOS (12,0)]
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface HKQuantitySeriesSampleBuilder
	{
		[Export ("initWithHealthStore:quantityType:startDate:device:")]
		NativeHandle Constructor (HKHealthStore healthStore, HKQuantityType quantityType, NSDate startDate, [NullAllowed] HKDevice device);

		[Export ("quantityType", ArgumentSemantic.Copy)]
		HKQuantityType QuantityType { get; }

		[Export ("startDate", ArgumentSemantic.Copy)]
		NSDate StartDate { get; }

		[NullAllowed, Export ("device", ArgumentSemantic.Copy)]
		HKDevice Device { get; }

		[Export ("insertQuantity:date:error:")]
		bool Insert (HKQuantity quantity, NSDate date, [NullAllowed] out NSError error);

		[Async, Protected]
		[Export ("finishSeriesWithMetadata:completion:")]
		void FinishSeries ([NullAllowed] NSDictionary metadata, HKQuantitySeriesSampleBuilderFinishSeriesDelegate completionHandler);

		[Async]
		[Wrap ("FinishSeries (metadata.GetDictionary (), completionHandler)")]
		void FinishSeries ([NullAllowed] HKMetadata metadata, HKQuantitySeriesSampleBuilderFinishSeriesDelegate completionHandler);

		[Watch (6,0), iOS (13,0)]
		[Async]
		[Export ("finishSeriesWithMetadata:endDate:completion:")]
		void FinishSeries ([NullAllowed] NSDictionary metadata, [NullAllowed] NSDate endDate, HKQuantitySeriesSampleBuilderFinishSeriesDelegate completionHandler);

		[Watch (6,0), iOS (13,0)]
		[Async]
		[Wrap ("FinishSeries (metadata.GetDictionary (), endDate, completionHandler)")]
		void FinishSeries ([NullAllowed] HKMetadata metadata, [NullAllowed] NSDate endDate, HKQuantitySeriesSampleBuilderFinishSeriesDelegate completionHandler);


		[Export ("discard")]
		void Discard ();

		[Watch (6,0), iOS (13,0)]
		[Export ("insertQuantity:dateInterval:error:")]
		bool Insert (HKQuantity quantity, NSDateInterval dateInterval, [NullAllowed] out NSError error);
	}

	[Watch (5,0), NoiOS]
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface HKLiveWorkoutDataSource
	{
		[Export ("typesToCollect", ArgumentSemantic.Copy)]
		NSSet<HKQuantityType> TypesToCollect { get; }

		[Export ("initWithHealthStore:workoutConfiguration:")]
		[DesignatedInitializer]
		NativeHandle Constructor (HKHealthStore healthStore, [NullAllowed] HKWorkoutConfiguration configuration);

		[Export ("enableCollectionForType:predicate:")]
		void EnableCollection (HKQuantityType quantityType, [NullAllowed] NSPredicate predicate);

		[Export ("disableCollectionForType:")]
		void DisableCollection (HKQuantityType quantityType);
	}

	[NoWatch, iOS (12,0)]
	[BaseType (typeof (NSObject), Name = "HKFHIRResource")]
	[DisableDefaultCtor]
	interface HKFhirResource : NSSecureCoding, NSCopying
	{
		[Internal]
		[Export ("resourceType")]
		NSString _ResourceType { get; }

		HKFhirResourceType ResourceType { [Wrap ("HKFhirResourceTypeExtensions.GetValue (_ResourceType)")] get; }

		[Export ("identifier")]
		string Identifier { get; }

		[Export ("data", ArgumentSemantic.Copy)]
		NSData Data { get; }

		[NullAllowed, Export ("sourceURL", ArgumentSemantic.Copy)]
		NSUrl SourceUrl { get; }

		[iOS (14, 0)]
		[Export ("FHIRVersion", ArgumentSemantic.Copy)]
		HKFhirVersion FhirVersion { get; }
	}

	[Watch (5,0), iOS (12,0)]
	[Deprecated (PlatformName.iOS, 13, 0, message: "Use HKCumulativeQuantitySample instead.")]
	[Deprecated (PlatformName.WatchOS, 6, 0, message: "Use HKCumulativeQuantitySample instead.")]
	[DisableDefaultCtor]
	[BaseType (typeof (HKCumulativeQuantitySample))]
	interface HKCumulativeQuantitySeriesSample
	{
		[Export ("sum", ArgumentSemantic.Copy)]
		HKQuantity Sum { get; }
	}

	[Watch (6,0), iOS (13,0)]
	[BaseType (typeof (HKQuantitySample))]
	[DisableDefaultCtor]
	interface HKCumulativeQuantitySample
	{
		[Export ("sumQuantity", ArgumentSemantic.Copy)]
		HKQuantity SumQuantity { get; }
	}

	[NoWatch, iOS (12,0)]
	[DisableDefaultCtor]
	[BaseType (typeof(HKSample))]
	interface HKClinicalRecord : NSSecureCoding, NSCopying
	{
		[Export ("clinicalType", ArgumentSemantic.Copy)]
		HKClinicalType ClinicalType { get; }

		[Export ("displayName")]
		string DisplayName { get; }

		[NullAllowed, Export ("FHIRResource", ArgumentSemantic.Copy)]
		HKFhirResource FhirResource { get; }
	}

	interface IHKLiveWorkoutBuilderDelegate {}
	[Watch (5,0), NoiOS]
	[Protocol, Model]
	[BaseType (typeof(NSObject))]
	interface HKLiveWorkoutBuilderDelegate
	{
		[Abstract]
		[Export ("workoutBuilder:didCollectDataOfTypes:")]
		void DidCollectData (HKLiveWorkoutBuilder workoutBuilder, NSSet<HKSampleType> collectedTypes);

		[Abstract]
		[Export ("workoutBuilderDidCollectEvent:")]
		void DidCollectEvent (HKLiveWorkoutBuilder workoutBuilder);
	}

	[Watch (5,0), NoiOS]
	[DisableDefaultCtor]
	[BaseType (typeof(HKWorkoutBuilder))]
	interface HKLiveWorkoutBuilder
	{
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		IHKLiveWorkoutBuilderDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[NullAllowed, Export ("workoutSession", ArgumentSemantic.Weak)]
		HKWorkoutSession WorkoutSession { get; }

		[Export ("shouldCollectWorkoutEvents")]
		bool ShouldCollectWorkoutEvents { get; set; }

		[NullAllowed, Export ("dataSource", ArgumentSemantic.Strong)]
		HKLiveWorkoutDataSource DataSource { get; set; }

		[Export ("elapsedTime")]
		double ElapsedTime { get; }
	}

	[Watch (6,0), iOS (13,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface HKAudiogramSensitivityPoint
	{
		[Export ("frequency", ArgumentSemantic.Copy)]
		HKQuantity Frequency { get; }

		[NullAllowed, Export ("leftEarSensitivity", ArgumentSemantic.Copy)]
		HKQuantity LeftEarSensitivity { get; }

		[NullAllowed, Export ("rightEarSensitivity", ArgumentSemantic.Copy)]
		HKQuantity RightEarSensitivity { get; }

		[Static]
		[Export ("sensitivityPointWithFrequency:leftEarSensitivity:rightEarSensitivity:error:")]
		[return: NullAllowed]
		HKAudiogramSensitivityPoint GetSensitivityPoint (HKQuantity frequency, [NullAllowed] HKQuantity leftEarSensitivity, [NullAllowed] HKQuantity rightEarSensitivity, [NullAllowed] out NSError error);
	}

	[Watch (6,0), iOS (13,0)]
	[BaseType (typeof (HKSample))]
	[DisableDefaultCtor]
	interface HKAudiogramSample
	{
		[Export ("sensitivityPoints", ArgumentSemantic.Copy)]
		HKAudiogramSensitivityPoint[] SensitivityPoints { get; }

		[Static]
		[Export ("audiogramSampleWithSensitivityPoints:startDate:endDate:metadata:")]
		HKAudiogramSample GetAudiogramSample (HKAudiogramSensitivityPoint[] sensitivityPoints, NSDate startDate, NSDate endDate, [NullAllowed] NSDictionary<NSString, NSObject> metadata);
	}

	[Watch (6,0), iOS (13,0)]
	[BaseType (typeof (HKQuantitySample))]
	[DisableDefaultCtor]
	interface HKDiscreteQuantitySample
	{
		[Export ("minimumQuantity", ArgumentSemantic.Copy)]
		HKQuantity Minimum { get; }

		[Export ("averageQuantity", ArgumentSemantic.Copy)]
		HKQuantity Average { get; }

		[Export ("maximumQuantity", ArgumentSemantic.Copy)]
		HKQuantity Maximum { get; }

		[Export ("mostRecentQuantity", ArgumentSemantic.Copy)]
		HKQuantity MostRecent { get; }

		[Export ("mostRecentQuantityDateInterval", ArgumentSemantic.Copy)]
		NSDateInterval MostRecentDateInterval { get; }
	}
	
	[iOS (13,0)]
	[Watch (6,0)]
	[BaseType (typeof (HKSeriesSample))]
	[DisableDefaultCtor]
	interface HKHeartbeatSeriesSample : NSSecureCoding {}
	
	delegate void HKHeartbeatSeriesBuilderCompletionHandler (bool success, NSError error);

	[Watch (6,0), iOS (13,0)]
	[BaseType (typeof (HKSeriesBuilder))]
	[DisableDefaultCtor]
	interface HKHeartbeatSeriesBuilder
	{
		[Static]
		[Export ("maximumCount")]
		nuint MaximumCount { get; }

		[Export ("initWithHealthStore:device:startDate:")]
		[DesignatedInitializer]
		NativeHandle Constructor (HKHealthStore healthStore, [NullAllowed] HKDevice device, NSDate startDate);

		[Export ("addHeartbeatWithTimeIntervalSinceSeriesStartDate:precededByGap:completion:")]
		[Async]
		void AddHeartbeat (double timeInterval, bool precededByGap, HKHeartbeatSeriesBuilderCompletionHandler completion);

		[Export ("addMetadata:completion:")]
		[Async]
		void AddMetadata (NSDictionary<NSString, NSObject> metadata, HKHeartbeatSeriesBuilderCompletionHandler completion);

		[Export ("finishSeriesWithCompletion:")]
		[Async]
		void FinishSeries (Action<HKHeartbeatSeriesSample, NSError> completion);
	}

	delegate void HKHeartbeatSeriesQueryDataHandler (HKHeartbeatSeriesQuery query, double timeSinceSeriesStart, bool precededByGap, bool done, NSError error);

	[Watch (6,0), iOS (13,0)]
	[BaseType (typeof (HKQuery))]
	interface HKHeartbeatSeriesQuery
	{
		[Export ("initWithHeartbeatSeries:dataHandler:")]
		[DesignatedInitializer]
		NativeHandle Constructor (HKHeartbeatSeriesSample heartbeatSeries, HKHeartbeatSeriesQueryDataHandler dataHandler);
	}

	[Watch (7,0), iOS (14,0)]
	[BaseType (typeof (HKSample))]
	[DisableDefaultCtor] // NSInvalidArgumentException Reason: The -init method is not available on HKElectrocardiogram
	interface HKElectrocardiogram
	{
		[Export ("numberOfVoltageMeasurements")]
		nint NumberOfVoltageMeasurements { get; }

		[NullAllowed, Export ("samplingFrequency", ArgumentSemantic.Copy)]
		HKQuantity SamplingFrequency { get; }

		[Export ("classification", ArgumentSemantic.Assign)]
		HKElectrocardiogramClassification Classification { get; }

		[NullAllowed, Export ("averageHeartRate", ArgumentSemantic.Copy)]
		HKQuantity AverageHeartRate { get; }

		[Export ("symptomsStatus", ArgumentSemantic.Assign)]
		HKElectrocardiogramSymptomsStatus SymptomsStatus { get; }
	}

	delegate void HKElectrocardiogramQueryDataHandler (HKElectrocardiogramQuery query, HKElectrocardiogramVoltageMeasurement voltageMeasurement, bool done, NSError error);

	[Watch (7,0), iOS (14,0)]
	[BaseType (typeof (HKQuery))]
	[DisableDefaultCtor]
	interface HKElectrocardiogramQuery
	{

		[Export ("initWithElectrocardiogram:dataHandler:")]
		[DesignatedInitializer]
		NativeHandle Constructor (HKElectrocardiogram electrocardiogram, HKElectrocardiogramQueryDataHandler dataHandler);
	}

	[Watch (7,0), iOS (14,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface HKElectrocardiogramVoltageMeasurement
	{
		[Export ("timeSinceSampleStart")]
		double TimeSinceSampleStart { get; }

		[Export ("quantityForLead:")]
		[return: NullAllowed]
		HKQuantity GetQuantity (HKElectrocardiogramLead lead);
	}

	[NoWatch, iOS (14,0)]
	[BaseType (typeof (NSObject), Name = "HKFHIRVersion")]
	[DisableDefaultCtor]
	interface HKFhirVersion : NSCopying, NSSecureCoding
	{
		[Export ("majorVersion")]
		nint MajorVersion { get; }

		[Export ("minorVersion")]
		nint MinorVersion { get; }

		[Export ("patchVersion")]
		nint PatchVersion { get; }

		[Export ("FHIRRelease", ArgumentSemantic.Strong)]
		string FhirRelease { get; }

		[Export ("stringRepresentation")]
		string StringRepresentation { get; }

		[Static]
		[Export ("versionFromVersionString:error:")]
		[return: NullAllowed]
		HKFhirVersion GetVersion (string versionString, [NullAllowed] out NSError errorOut);

		[Static]
		[Export ("primaryDSTU2Version")]
		HKFhirVersion PrimaryDstu2Version { get; }

		[Static]
		[Export ("primaryR4Version")]
		HKFhirVersion PrimaryR4Version { get; }
	}

	[Watch (7,0), iOS (14,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface HKActivityMoveModeObject : NSCopying, NSSecureCoding {

		[Export ("activityMoveMode")]
		HKActivityMoveMode ActivityMoveMode { get; }
	}

	[Watch (7,2), iOS (14,3)]
	[Native]
	enum HKCategoryValueContraceptive : long {
		Unspecified = 1,
		Implant,
		Injection,
		IntrauterineDevice,
		IntravaginalRing,
		Oral,
		Patch,
	}

	[Watch (7,2), iOS (14,3)]
	[Native]
	enum HKCategoryValueLowCardioFitnessEvent : long {
		LowFitness = 1,
	}

	[Watch (8,0), iOS (15,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface HKQueryDescriptor : NSCopying, NSSecureCoding
	{
		[Export ("sampleType", ArgumentSemantic.Copy)]
		HKSampleType SampleType { get; }

		[NullAllowed, Export ("predicate", ArgumentSemantic.Copy)]
		NSPredicate Predicate { get; }

		[Export ("initWithSampleType:predicate:")]
		[DesignatedInitializer]
		NativeHandle Constructor (HKSampleType sampleType, [NullAllowed] NSPredicate predicate);
	}

	[NoWatch, iOS (15,0)]
	[BaseType (typeof (HKSample))]
	[DisableDefaultCtor]
	interface HKVerifiableClinicalRecord
	{
		[Export ("recordTypes", ArgumentSemantic.Copy)]
		string[] RecordTypes { get; }

		[Export ("issuerIdentifier")]
		string IssuerIdentifier { get; }

		[Export ("subject", ArgumentSemantic.Copy)]
		HKVerifiableClinicalRecordSubject Subject { get; }

		[Export ("issuedDate", ArgumentSemantic.Copy)]
		NSDate IssuedDate { get; }

		[Export ("relevantDate", ArgumentSemantic.Copy)]
		NSDate RelevantDate { get; }

		[NullAllowed, Export ("expirationDate", ArgumentSemantic.Copy)]
		NSDate ExpirationDate { get; }

		[Export ("itemNames", ArgumentSemantic.Copy)]
		string[] ItemNames { get; }

		[Export ("JWSRepresentation", ArgumentSemantic.Copy)]
		NSData JwsRepresentation { get; }
	}

	delegate void HKVerifiableClinicalRecordQueryResultHandler (HKVerifiableClinicalRecordQuery query, NSArray<HKVerifiableClinicalRecord> records, NSError error);

	[NoWatch, iOS (15,0)]
	[BaseType (typeof (HKQuery))]
	[DisableDefaultCtor]
	interface HKVerifiableClinicalRecordQuery
	{
		[Export ("recordTypes", ArgumentSemantic.Copy)]
		string[] RecordTypes { get; }

		[Export ("initWithRecordTypes:predicate:resultsHandler:")]
		NativeHandle Constructor (string[] recordTypes, [NullAllowed] NSPredicate predicate, HKVerifiableClinicalRecordQueryResultHandler handler);
	}

	[NoWatch, iOS (15,0)]
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor]
	interface HKVerifiableClinicalRecordSubject : NSSecureCoding, NSCopying
	{
		[Export ("fullName")]
		string FullName { get; }

		[NullAllowed, Export ("dateOfBirthComponents", ArgumentSemantic.Copy)]
		NSDateComponents DateOfBirthComponents { get; }
	}

}
