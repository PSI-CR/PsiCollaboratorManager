
---------------------------------------------------------------------------------------------------
-------------------------------------------Delete funtions-----------------------------------------
---------------------------------------------------------------------------------------------------

DO $$ 
DECLARE 
    r RECORD;
BEGIN
    FOR r IN 
        (SELECT routine_name
         FROM information_schema.routines
         WHERE routine_schema = 'public'
         AND routine_type = 'FUNCTION')
    LOOP
        EXECUTE 'DROP FUNCTION IF EXISTS public.' || r.routine_name || ' CASCADE';
    END LOOP;
END $$;

---------------------------------------------------------------------------------------------------
--------------------------------------------Funtions-----------------------------------------------
---------------------------------------------------------------------------------------------------

CREATE OR REPLACE FUNCTION public.select_data_schedule_workingday()
 RETURNS TABLE(scheduleid integer, name character varying, assigned boolean, workingdayname character varying, workingdaydescription character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT 
        s.scheduleid,
        s.schedulename AS name,
        s.assignedschedule AS assigned,
        wd.workingdayname,
        wd.description AS workingdaydescription
    FROM public.schedule s
    JOIN public.schedule_workingday wd ON s.workingdayid = wd.workingdayid;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_highest_operatornumber()
 RETURNS integer
 LANGUAGE plpgsql
AS $function$
DECLARE
    highest_operator_number INTEGER;
BEGIN
    SELECT MAX(OperatorNumber) INTO highest_operator_number FROM Collaborator;
    IF highest_operator_number IS NULL THEN
        highest_operator_number := 1;
    END IF;

    RETURN highest_operator_number;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_password_policy_by_id(param_passwordpolicyid integer)
 RETURNS SETOF sessionpasswordpolicy
 LANGUAGE plpgsql
AS $function$
BEGIN
	RETURN QUERY SELECT * FROM SessionPasswordPolicy WHERE passwordpolicyid = param_passwordpolicyid;
END;
$function$;

CREATE OR REPLACE FUNCTION public.select_schedule_all()
 RETURNS TABLE(scheduleid integer, schedulename character varying, workingdayname character varying, workingdaydescription character varying, assignedschedule boolean)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT
        s.scheduleid,
        s.schedulename as name,
        w.workingdayname,
        w.description as workingdaydescription,
        s.assignedschedule as assigned
    FROM
        public.schedule s
    LEFT JOIN
        public.schedule_workingday w ON s.workingdayid = w.workingdayid;
END;
$function$;

CREATE OR REPLACE FUNCTION public.select_schedule_all_basic()
 RETURNS TABLE(scheduleid integer, name character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT
        s.scheduleid,
        s.schedulename as name
    FROM
        public.schedule s;
END;
$function$;

CREATE OR REPLACE FUNCTION public.select_schedule_by_scheduledailyid(param_scheduledailyid integer)
 RETURNS TABLE(scheduledailyid integer, dayid integer, begintime timestamp without time zone, endtime timestamp without time zone)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT sd.scheduledailyid, sd.dayid, sd.begintime, sd.endtime
    FROM schedule_daily sd
    WHERE sd.scheduledailyid = param_scheduleDailyId;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_schedule_check_in_status_all()
 RETURNS TABLE(checkinstatusid integer, typecheckinstatus character varying, labelcheckinstatus character varying, description character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT 
        s.checkinstatusid,
        s.typecheckinstatus,
        s.labelcheckinstatus,
        s.description
    FROM public.schedule_checkinstatus s;
END;
$function$;

CREATE OR REPLACE FUNCTION public.select_schedule_check_out_status_all()
 RETURNS TABLE(checkoutstatusid integer, typecheckoutstatus character varying, labelcheckoutstatus character varying, description character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT 
        s.checkoutstatusid,
        s.typecheckoutstatus,
        s.labelcheckoutstatus,
        s.description
    FROM public.schedule_checkoutstatus s;
END;
$function$;

CREATE OR REPLACE FUNCTION public.select_schedule_daily_by_scheduleid(param_scheduleid integer)
 RETURNS TABLE(scheduledailyid integer, dayname character varying, begintime timestamp without time zone, endtime timestamp without time zone)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT
        sd.scheduledailyid,
        d.dayname, -- Aquí se devuelve como "dayname"
        sd.begintime,
        sd.endtime
    FROM
        public.schedule_daysbyschedule ds
    INNER JOIN
        public.schedule_daily sd ON ds.scheduledailyid = sd.scheduledailyid
    INNER JOIN
        public.schedule_days d ON sd.dayid = d.dayid
    WHERE
        ds.scheduleid = param_scheduleid;
END;
$function$;

CREATE OR REPLACE FUNCTION public.select_schedule_days_by_scheduledailyid(param_scheduledailyid integer)
 RETURNS TABLE(daybyscheduleid integer, scheduleid integer, scheduledailyid integer)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT sdbs.daybyscheduleid, sdbs.scheduleid, sdbs.scheduledailyid
    FROM schedule_daysbyschedule sdbs
    WHERE sdbs.scheduledailyid = param_scheduledailyid;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_schedule_days_by_scheduleid(param_scheduleid integer)
 RETURNS TABLE(daybyscheduleid integer, scheduleid integer, scheduledailyid integer)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT sds.daybyscheduleid, sds.scheduleid, sds.scheduledailyid
    FROM public.schedule_daysbyschedule sds
    WHERE sds.scheduleid = param_scheduleId;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_schedule_details_by_collaborator(param_collaboratorid integer)
 RETURNS TABLE(scheduledailyid integer, dayid integer, begintime timestamp without time zone, endtime timestamp without time zone, dayname character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT 
        sd.scheduledailyid,
        sd.dayid,
        sd.begintime,
        sd.endtime,
        sdname.dayname
    FROM 
        collaborator_schedule_history csh
    JOIN 
        schedule s ON csh.scheduleid = s.scheduleid
    JOIN 
        schedule_daysbyschedule sds ON s.scheduleid = sds.scheduleid
    JOIN 
        schedule_daily sd ON sds.scheduledailyid = sd.scheduledailyid
    JOIN 
        schedule_days sdname ON sd.dayid = sdname.dayid
    WHERE 
        csh.collaboratorid = param_collaboratorid  
        AND csh.active = true;  
END;
$function$;

 CREATE OR REPLACE FUNCTION public.update_attend(param_attendanceid integer, param_collaboratorid integer, param_checkin timestamp without time zone, param_checkout timestamp without time zone, param_checkinstatus integer, param_checkoutstatus integer, param_commentcheckin character varying, param_isopencheckin boolean, param_labelcheckinstatus text, param_labelcheckoutstatus text)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    UPDATE public.collaborator_attend
    SET 
		collaboratorid = param_collaboratorid,
        checkin = param_checkin,
        checkout = param_checkout,
        checkinstatus = param_checkinstatus,
        checkoutstatus = param_checkoutstatus,
        commentcheckin = param_commentcheckin,
        isopencheckin = param_isopencheckin
    WHERE 
        attendanceid = param_attendanceid;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.assign_schedule(param_scheduleid integer, param_collaboratorid integer)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    INSERT INTO public.collaborator_schedule_history (collaboratorid, scheduleid, active, assigndate, dismissdate)
    VALUES (param_collaboratorid, param_scheduleid, TRUE, NOW(), '0001-01-01 00:00:00 AD');
END;
$function$;

 CREATE OR REPLACE FUNCTION public.delete_announcement_art(param_announcement_art_id integer)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    DELETE FROM Announcement_Art WHERE AnnouncementId = param_announcement_art_id;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.delete_attend_by_id(param_attendanceid integer)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    DELETE FROM public.collaborator_attend
    WHERE attendanceid = param_attendanceid;
END;
$function$;

CREATE OR REPLACE FUNCTION public.delete_collaborator(param_collaboratorid integer)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    DELETE FROM Collaborator_BankAccount WHERE CollaboratorId = param_collaboratorId;
    DELETE FROM Collaborator_HealthCondition WHERE CollaboratorId = param_collaboratorId;
    DELETE FROM Collaborator_Picture WHERE CollaboratorId = param_collaboratorId;
    DELETE FROM Collaborator_EmergencyContact WHERE CollaboratorId = param_collaboratorId;
	DELETE FROM Collaborator_passwordhistory WHERE CollaboratorId = param_collaboratorId;
    DELETE FROM Collaborator WHERE CollaboratorId = param_collaboratorId;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.delete_working_day(param_working_day_id integer)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF (SELECT workingdayassigned FROM Schedule_WorkingDay WHERE WorkingDayId = param_working_day_id) THEN
        RAISE EXCEPTION 'No se puede eliminar el día laboral asignado.';
    ELSE
        DELETE FROM Schedule_WorkingDay WHERE WorkingDayId = param_working_day_id;
    END IF;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.dismiss_schedule(param_collaboratorid integer)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    UPDATE public.collaborator_schedule_history
    SET active = FALSE,
        dismissdate = NOW()
    WHERE collaboratorschedulehistoryid = (
        SELECT collaboratorschedulehistoryid
        FROM public.collaborator_schedule_history
        WHERE collaboratorid = param_collaboratorid
          AND active = TRUE
        ORDER BY assigndate DESC
        LIMIT 1
    );
END;
$function$;

  CREATE OR REPLACE FUNCTION public.get_all_schedule_daysbyschedule()
 RETURNS TABLE(daybyscheduleid integer, scheduleid integer, scheduledailyid integer)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT 
        sds.daybyscheduleid,
        sds.scheduleid,
        sds.scheduledailyid
    FROM 
        public.schedule_daysbyschedule AS sds;
END;
$function$;

CREATE OR REPLACE FUNCTION public.get_attendance_by_collaborator_and_dates(param_collaboratorid integer, param_start_date timestamp without time zone, param_end_date timestamp without time zone)
 RETURNS TABLE(attendanceid integer, collaboratorid integer, checkin timestamp without time zone, checkout timestamp without time zone, checkinstatus integer, checkoutstatus integer, labelcheckinstatus character varying, labelcheckoutstatus character varying, commentcheckin character varying, isopencheckin boolean, ipaddress character varying, physicaladdressequipment character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF param_start_date::date = param_end_date::date THEN
        -- Si las fechas son iguales, buscar solo los registros de esa fecha específica
        RETURN QUERY
        SELECT
            a.attendanceid,
            a.collaboratorid,
            a.checkin,
            a.checkout,
            a.checkinstatus,
            a.checkoutstatus,
            cs.labelcheckinstatus,  -- Etiqueta de estado de check-in
            ccs.labelcheckoutstatus, -- Etiqueta de estado de check-out
            a.commentcheckin,
            a.isopencheckin,
            a.ipaddress,
            a.physicaladdressequipment
        FROM
            public.collaborator_attend a
        LEFT JOIN
            public.schedule_checkinstatus cs ON a.checkinstatus = cs.checkinstatusid  -- Join para el estado de check-in
        LEFT JOIN
            public.schedule_checkoutstatus ccs ON a.checkoutstatus = ccs.checkoutstatusid  -- Join para el estado de check-out
        WHERE
            a.collaboratorid = param_collaboratorid
            AND a.checkin::date = param_start_date::date;
    ELSE
        -- Si las fechas son diferentes, buscar dentro del rango de fechas
        RETURN QUERY
        SELECT
            a.attendanceid,
            a.collaboratorid,
            a.checkin,
            a.checkout,
            a.checkinstatus,
            a.checkoutstatus,
            cs.labelcheckinstatus,  -- Etiqueta de estado de check-in
            ccs.labelcheckoutstatus, -- Etiqueta de estado de check-out
            a.commentcheckin,
            a.isopencheckin,
            a.ipaddress,
            a.physicaladdressequipment
        FROM
            public.collaborator_attend a
        LEFT JOIN
            public.schedule_checkinstatus cs ON a.checkinstatus = cs.checkinstatusid  -- Join para el estado de check-in
        LEFT JOIN
            public.schedule_checkoutstatus ccs ON a.checkoutstatus = ccs.checkoutstatusid  -- Join para el estado de check-out
        WHERE
            a.collaboratorid = param_collaboratorid
            AND a.checkin >= param_start_date
            AND a.checkout <= param_end_date;
    END IF;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.get_dayids_by_scheduleid(param_scheduleid integer)
 RETURNS TABLE(dayid integer)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT sd.dayid
    FROM public.schedule_daily sd
    JOIN public.schedule_daysbyschedule sds
    ON sd.scheduledailyid = sds.scheduledailyid
    WHERE sds.scheduleid = param_scheduleId;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.getallannotationtypes()
 RETURNS TABLE(annotationtypeid integer, typename character varying, valueinscore boolean, visibletocollaborator boolean, percentage double precision)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT
        at.AnnotationTypeId,
        at.TypeName,
        at.ValueInScore,
        at.VisibleToCollaborator,
        at.Percentage
    FROM
        AnnotationType at;
END;
$function$;

CREATE OR REPLACE FUNCTION public.getannotationtypebyid(param_annotationtypeid integer)
 RETURNS TABLE(annotationtypeid integer, typename character varying, valueinscore boolean, visibletocollaborator boolean, percentage double precision)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT
        at.AnnotationTypeId,
        at.TypeName,
        at.ValueInScore,
        at.VisibleToCollaborator,
        at.Percentage
    FROM
        AnnotationType at
    WHERE
        at.AnnotationTypeId = param_AnnotationTypeId;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.insert_annotation(param_annotationid integer, param_collaboratorids integer[], param_annotationtypeid integer, param_userid integer, param_filedata text, param_filename character varying, param_filetype character varying, param_note text, param_date timestamp without time zone)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
DECLARE
    collaborator_id integer;
    new_attachment_id integer;
BEGIN
    IF COALESCE(param_FileData, '') <> '' THEN
        INSERT INTO Annotation_Attachment (FileData, FileName, FileType)
        VALUES (param_FileData, param_FileName, param_FileType)
        RETURNING AttachmentId INTO new_attachment_id;
    ELSE
        new_attachment_id := NULL;
    END IF;

    FOREACH collaborator_id IN ARRAY param_collaboratorIds
    LOOP
        INSERT INTO Annotation (CollaboratorId, AnnotationTypeId, UserAccountId, AttachmentId, Note, AnnotationDate)
        VALUES (collaborator_id, param_annotationTypeId, param_userid, new_attachment_id, param_note, param_date);
    END LOOP;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_all_collaborators_name()
 RETURNS TABLE(collaboratorid integer, firstname character varying, lastname character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT 
        c.CollaboratorId,
        c.FirstName,
        c.LastName
    FROM 
        Collaborator c;
END;
$function$;

CREATE OR REPLACE FUNCTION public.select_all_collaborators_operator()
 RETURNS TABLE(collaboratorid integer, firstname character varying, lastname character varying, operatornumber integer)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT 
        c.CollaboratorId,
        c.FirstName,
        c.LastName,
		c.OperatorNumber
    FROM 
        Collaborator c;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_all_currencytype()
 RETURNS TABLE(currencytypeid integer, name character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY 
    SELECT 
        ct.CurrencyTypeId,
        ct.Name
    FROM 
        CurrencyType ct;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_all_days()
 RETURNS TABLE(dayid integer, dayname character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY SELECT schedule_days.dayid, schedule_days.dayname FROM schedule_days;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_all_districts_by_canton(param_cantonid integer)
 RETURNS TABLE(districtid integer, name character varying, description character varying, postalcode character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT
        ad.DistrictId,
        ad.DistrictName,
        ad.Description,
        ad.PostalCode
    FROM
        Address_District ad
    WHERE
        ad.CantonId = param_CantonId;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_all_provinces()
 RETURNS TABLE(provinceid integer, name character varying, description character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT
        ap.ProvinceId,
        ap.ProvinceName,
        ap.Description
    FROM
        Address_Province ap;
END;
$function$;

CREATE OR REPLACE FUNCTION public.select_all_schedule_days()
 RETURNS TABLE(dayid integer, dayname character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY 
    SELECT sd.dayid, sd.dayname 
    FROM schedule_days sd;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_all_user_accounts()
 RETURNS TABLE(useraccountid integer, username character varying, firstname character varying, lastname character varying, email character varying, telephone1 character varying, needpasswordchange boolean, islockedout boolean, lockoutendtime timestamp without time zone, isactive boolean)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT
        ua.UserAccountId,
        ua.UserName,
        ua.FirstName,
        ua.LastName,
        ua.Email,
        ua.Telephone1,
        ua.NeedPasswordChange,
        ua.IsLockedOut,
        ua.LockoutEndTime,
        ua.IsActive
    FROM
        UserAccount ua;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_all_working_day_by_id(param_workingdayid integer)
 RETURNS TABLE(workingdayid integer, name character varying, description character varying, maxdays integer, maxhours integer, starttime timestamp without time zone, endtime timestamp without time zone, recordtime timestamp without time zone, accumulative boolean)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT 
        wd.workingdayid, 
        wd.workingdayname, 
        wd.description, 
        wd.workingdaymaxdays, 
        wd.workingdaymaxhours,  
        (CURRENT_DATE + wd.workingdaystarttime)::timestamp without time zone, 
        (CURRENT_DATE + wd.workingdayendtime)::timestamp without time zone, 
        wd.workingdayrecordtime, 
        wd.workingdayaccumulative
    FROM public.schedule_workingday wd
    WHERE wd.workingdayid = param_workingdayid;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_annotation_by_id(param_annotationid integer)
 RETURNS TABLE(annotationid integer, collaboratorfirstname character varying, collaboratorlastname character varying, collaboratordnicollaborator character varying, collaboratoroperatornumber integer, annotationtypename character varying, note text, date date, collaboratorpicture text)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY 
    SELECT 
        A.AnnotationId,
        C.FirstName as CollaboratorFirstName,
        C.LastName as CollaboratorLastName,
        C.DNICollaborator as CollaboratorDNICollaborator,
        C.OperatorNumber as CollaboratorOperatorNumber,
        AT.TypeName as AnnotationTypeName,
        A.Note,
        A.AnnotationDate as Date,
		CP.Picture as CollaboratorPicture
    FROM 
        Annotation A
    INNER JOIN 
        Collaborator C ON A.CollaboratorId = C.CollaboratorId
	INNER JOIN 
		Collaborator_Picture CP ON A.CollaboratorId = CP.CollaboratorId
    INNER JOIN 
        AnnotationType AT ON A.AnnotationTypeId = AT.AnnotationTypeId
    LEFT JOIN 
        Annotation_Attachment AA ON A.AttachmentId = AA.AttachmentId
    WHERE 
        A.AnnotationId = param_annotationId;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_announcement_art_by_date()
 RETURNS TABLE(announcementid integer, image character varying, begindatepublication timestamp without time zone, enddatepublication timestamp without time zone, description character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY 
    SELECT 
        aa.announcementid,
        aa.imageannouncement as image,
        aa.begindatepublication,
        aa.enddatepublication,
        aa.description
    FROM 
        announcement_art aa
    WHERE 
        aa.begindatepublication::date <= NOW()::date AND
        NOW()::date <= aa.enddatepublication::date;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_announcement_art_by_id(param_announcement_art_id integer)
 RETURNS TABLE(announcementid integer, image character varying, begindatepublication timestamp without time zone, enddatepublication timestamp without time zone, description character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY SELECT * FROM Announcement_Art as aa WHERE aa.AnnouncementId = param_announcement_art_id;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.insert_attend(param_collaboratorid integer, param_checkin timestamp without time zone, param_checkout timestamp without time zone, param_checkinstatus integer, param_checkoutstatus integer, param_commentcheckin character varying, param_isopencheckin boolean, param_ipaddress character varying, param_physicaladdressequipment character varying)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    INSERT INTO public.collaborator_attend (
        collaboratorid, 
        checkin, 
        checkout, 
        checkinstatus, 
        checkoutstatus, 
        commentcheckin, 
        isopencheckin, 
        ipaddress, 
        physicaladdressequipment
    ) 
    VALUES (
        param_collaboratorid, 
        param_checkin, 
        param_checkout, 
        param_checkinstatus, 
        param_checkoutstatus, 
        param_commentcheckin, 
        param_isopencheckin, 
        param_ipaddress, 
        param_physicaladdressequipment
    );
END;
$function$;

  CREATE OR REPLACE FUNCTION public.insert_login_attempt_collaborator(param_collaboratorid integer, param_issuccess boolean, param_ipaddress inet, param_applicationname text)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    INSERT INTO collaborator_connectionattempt(collaboratorid,issuccess,logintime,ipaddress, applicationname) VALUES(param_CollaboratorId, param_issuccess, timezone('America/Costa_Rica', now()), param_ipaddress, param_applicationname);
END;
$function$;

  CREATE OR REPLACE FUNCTION public.insert_login_attempt_user(param_useraccountid integer, param_issuccess boolean, param_ipaddress inet, param_applicationname text)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    INSERT INTO user_connectionattempt(useraccountid,issuccess,logintime,ipaddress, applicationname) VALUES(param_useraccountid, param_issuccess, timezone('America/Costa_Rica', now()), param_ipaddress, param_applicationname);
END;
$function$;

  CREATE OR REPLACE FUNCTION public.insert_user_account(param_username character varying, param_firstname character varying, param_lastname character varying, param_email character varying, param_telephone1 character varying, param_needpasswordchange boolean, param_islockedout boolean, param_lockoutendtime timestamp without time zone, param_isactive boolean, param_passwordhash text)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
DECLARE
    new_user_account_id INTEGER;
    user_count INTEGER;
BEGIN
    -- Check for unique UserName
    SELECT COUNT(*) INTO user_count
    FROM UserAccount
    WHERE UserName = param_UserName;
    
    IF user_count > 0 THEN
        RAISE EXCEPTION 'UserName % is already in use', param_UserName;
    END IF;
    
    -- Check for unique Email
    SELECT COUNT(*) INTO user_count
    FROM UserAccount
    WHERE Email = param_Email;
    
    IF user_count > 0 THEN
        RAISE EXCEPTION 'Email % is already in use', param_Email;
    END IF;

    -- Insert into UserAccount
    INSERT INTO UserAccount (
        UserName, 
        FirstName, 
        LastName, 
        Email, 
        Telephone1, 
        NeedPasswordChange, 
        IsLockedOut, 
        LockoutEndTime, 
        IsActive
    ) VALUES (
        param_UserName, 
        param_FirstName, 
        param_LastName, 
        param_Email, 
        param_Telephone1, 
        param_NeedPasswordChange, 
        param_IsLockedOut, 
        param_LockoutEndTime, 
        param_IsActive
    ) RETURNING UserAccountId INTO new_user_account_id;
    
    -- Insert into User_PasswordHistory
    INSERT INTO User_PasswordHistory (
        UserAccountId, 
        PasswordHash, 
        PasswordChangeDate, 
        IsActual
    ) VALUES (
        new_user_account_id, 
        param_PasswordHash, 
        NOW(), 
        TRUE
    );
END;
$function$;

  CREATE OR REPLACE FUNCTION public.insert_working_day(param_workingdayid integer, param_name character varying, param_description character varying, param_maxdays integer, param_maxhours integer, param_starttime timestamp without time zone, param_endtime timestamp without time zone, param_recordtime timestamp without time zone, param_accumulative boolean, param_assigned boolean)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    INSERT INTO public.schedule_workingday(
        workingdayname, 
        description, 
        workingdaymaxdays, 
        workingdaymaxhours, 
        workingdaystarttime, 
        workingdayendtime, 
        workingdayrecordtime, 
        workingdayaccumulative, 
        workingdayassigned
    ) VALUES (
        param_name, 
        param_description, 
        param_maxdays, 
        param_maxhours, 
        param_starttime, 
        param_endtime, 
        param_recordtime, 
        param_accumulative, 
        param_assigned
    );
END;
$function$;

  CREATE OR REPLACE FUNCTION public.save_announcement_art(param_announcementartid integer, param_image character varying, param_begindatepublication timestamp without time zone, param_enddatepublication timestamp without time zone, param_description character varying)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    UPDATE public.announcement_art
    SET 
        imageannouncement = param_image,
        begindatepublication = param_begindatepublication,
        enddatepublication = param_enddatepublication,
        description = param_description
    WHERE announcementid = param_announcementartid;
    IF NOT FOUND THEN
        INSERT INTO public.announcement_art (
            imageannouncement,
            begindatepublication,
            enddatepublication,
            description
        ) VALUES (
            param_image,
            param_begindatepublication,
            param_enddatepublication,
            param_description
        );
    END IF;
END;
$function$;;

  CREATE OR REPLACE FUNCTION public.save_collaborator(param_collaboratorid integer, param_firstname character varying, param_lastname character varying, param_operatornumber integer, param_email character varying, param_dnicollaborator character varying, param_dateofbirth timestamp without time zone, param_gender integer, param_parent boolean, param_maritalstatusid integer, param_telephone1 character varying, param_telephone2 character varying, param_curriculumfile text, param_rfidcode character varying, param_needpasswordchange boolean, param_islockedout boolean, param_lockoutendtime timestamp without time zone, param_password text, param_createuseraccount boolean, param_isactive boolean, param_districtid integer, param_cantonid integer, param_provinceid integer, param_addressline character varying, param_bankid integer, param_currencytypeid integer, param_numberbankaccount character varying, param_ibanaccount character varying, param_diseases character varying, param_takingmedications boolean, param_note character varying, param_picture text, param_emergencycontacts text[])
 RETURNS void
 LANGUAGE plpgsql
AS $function$
DECLARE
    collaborator_id INTEGER;
    bank_account_id INTEGER;
    health_condition_id INTEGER;
    picture_id INTEGER;
    contact_row text;
BEGIN
    BEGIN
        IF param_collaboratorId <> 0 THEN
            UPDATE Collaborator
            SET 
                OperatorNumber = param_operatornumber,
                FirstName = param_firstname,
                LastName = param_lastname,
                Email = param_email,
                DNICollaborator = param_dnicollaborator,
                DateOfBirth = param_dateofbirth,
                Gender = param_gender,
                Parent = param_parent,
                MaritalStatusId = param_maritalstatusid,
                Telephone1 = param_telephone1,
                Telephone2 = param_telephone2,
                CurriculumFile = param_curriculumfile,
                RFIDCode = param_rfidcode,
                NeedPasswordChange = param_needpasswordchange,
                IsLockedOut = param_islockedout,
                LockOutEndTime = param_lockoutendtime,
                IsActive = param_isactive,
                DistrictId = param_districtid,
                CantonId = param_cantonid,
                ProvinceId = param_provinceid,
                AddressLine = param_addressline
            WHERE CollaboratorId = param_collaboratorId;
            collaborator_id := param_collaboratorId;
        ELSE
            INSERT INTO Collaborator(
                OperatorNumber, FirstName, LastName, Email, DNICollaborator, DateOfBirth, Gender, Parent, MaritalStatusId, Telephone1, Telephone2,
                CurriculumFile, RFIDCode, NeedPasswordChange, IsLockedOut, LockOutEndTime, IsActive, DistrictId, CantonId, ProvinceId, AddressLine)
            VALUES (
                param_operatornumber, param_firstname, param_lastname, param_email, param_dnicollaborator, param_dateofbirth, param_gender, param_parent, param_maritalstatusid, param_telephone1, param_telephone2,
                param_curriculumfile, param_rfidcode, param_needpasswordchange, param_islockedout, param_lockoutendtime, param_isactive, param_districtid, param_cantonid, param_provinceid, param_addressline)
            RETURNING CollaboratorId INTO collaborator_id;
        END IF;
        IF EXISTS (SELECT 1 FROM Collaborator_BankAccount WHERE CollaboratorId = collaborator_id) THEN
            UPDATE Collaborator_BankAccount
            SET 
                BankId = param_bankid,
                CurrencyTypeId = param_currencytypeid,
                NumberBankAccount = param_numberbankaccount,
                IbanAccount = param_ibanaccount
            WHERE CollaboratorId = collaborator_id;
        ELSE
            INSERT INTO Collaborator_BankAccount(
                CollaboratorId, BankId, CurrencyTypeId, NumberBankAccount, IbanAccount)
            VALUES (
                collaborator_id, param_bankid, param_currencytypeid, param_numberbankaccount, param_ibanaccount)
            RETURNING BankAccountId INTO bank_account_id;
        END IF;
        IF EXISTS (SELECT 1 FROM Collaborator_HealthCondition WHERE CollaboratorId = collaborator_id) THEN
            UPDATE Collaborator_HealthCondition
            SET 
                Diseases = param_diseases,
                TakingMedications = param_takingmedications,
                Note = param_note
            WHERE CollaboratorId = collaborator_id;
        ELSE
            INSERT INTO Collaborator_HealthCondition(
                CollaboratorId, Diseases, TakingMedications, Note)
            VALUES (
                collaborator_id, param_diseases, param_takingmedications, param_note)
            RETURNING HealthConditionId INTO health_condition_id;
        END IF;
        IF EXISTS (SELECT 1 FROM Collaborator_Picture WHERE CollaboratorId = collaborator_id) THEN
            UPDATE Collaborator_Picture
            SET 
                Picture = param_picture
            WHERE CollaboratorId = collaborator_id;
            picture_id := collaborator_id;
        ELSE
            INSERT INTO Collaborator_Picture(
                CollaboratorId, Picture)
            VALUES (
                collaborator_id, param_picture)
            RETURNING CollaboratorPictureId INTO picture_id;
        END IF;
		Delete from Collaborator_EmergencyContact where CollaboratorId = collaborator_id;
        FOR i IN 1..array_length(param_emergencycontacts, 1) LOOP
            contact_row := param_emergencycontacts[i];
			INSERT INTO Collaborator_EmergencyContact (
				CollaboratorId, FirstName, LastName, Relationship, Telephone1, Telephone2)
			VALUES (
				collaborator_id,
				(string_to_array(contact_row, ','))[1],
				(string_to_array(contact_row, ','))[2],
				(string_to_array(contact_row, ','))[3],
				(string_to_array(contact_row, ','))[4],
				(string_to_array(contact_row, ','))[5]
			);
        END LOOP;
		IF(param_createUserAccount) THEN
			UPDATE collaborator_passwordhistory Set isActual = false where CollaboratorId = collaborator_id;
			INSERT INTO collaborator_passwordhistory(CollaboratorId, PasswordHash, passwordchangedate, IsActual) VALUES (collaborator_id, param_password, now(), true);
		END IF;
    END;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_attend_by_collaboratorid(param_collaboratorid integer)
 RETURNS TABLE(attendanceid integer, collaboratorid integer, checkin timestamp without time zone, checkout timestamp without time zone, checkinstatus integer, checkoutstatus integer, commentcheckin character varying, isopencheckin boolean, ipaddress character varying, physicaladdressequipment character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY 
    SELECT 
        collaborator_attend.attendanceid, 
        collaborator_attend.collaboratorid, 
        collaborator_attend.checkin, 
        collaborator_attend.checkout, 
        collaborator_attend.checkinstatus, 
        collaborator_attend.checkoutstatus, 
        collaborator_attend.commentcheckin, 
        collaborator_attend.isopencheckin, 
        collaborator_attend.ipaddress, 
        collaborator_attend.physicaladdressequipment
    FROM 
        public.collaborator_attend
    WHERE 
        collaborator_attend.collaboratorid = param_collaboratorid;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.save_schedule(param_scheduleid integer, param_name character varying, param_workingdayid integer, param_scheduledailys text[])
 RETURNS void
 LANGUAGE plpgsql
AS $function$
DECLARE
    schedule_id INTEGER;
    daily_row text;
    daily_id INTEGER;
BEGIN
    BEGIN
        IF param_scheduleid <> 0 THEN
            UPDATE public.schedule
            SET 
                schedulename = param_name,
                workingdayid = param_workingdayid
            WHERE scheduleid = param_scheduleid;
            schedule_id := param_scheduleid;
        ELSE
            INSERT INTO public.schedule(
                schedulename, workingdayid)
            VALUES (
                param_name, param_workingdayid)
            RETURNING scheduleid INTO schedule_id;
        END IF;
        
        DELETE FROM public.schedule_daysbyschedule WHERE scheduleid = schedule_id;
        
        FOR i IN 1..array_length(param_scheduledailys, 1) LOOP
            daily_row := param_scheduledailys[i];
            INSERT INTO public.schedule_daily(
                dayid, begintime, endtime)
            VALUES (
                (string_to_array(daily_row, ','))[1]::integer,
                (string_to_array(daily_row, ','))[2]::timestamp,
                (string_to_array(daily_row, ','))[3]::timestamp)
            RETURNING scheduledailyid INTO daily_id;
            
            INSERT INTO public.schedule_daysbyschedule(
                scheduleid, scheduledailyid)
            VALUES (
                schedule_id, daily_id);
        END LOOP;
    EXCEPTION
        WHEN OTHERS THEN
            RAISE EXCEPTION 'An error occurred: %', SQLERRM;
    END;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.saveannotationtype(param_annotationtypeid integer, param_typename character varying, param_valueinscore boolean, param_visibletocollaborator boolean, param_percentage double precision)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF EXISTS (SELECT 1 FROM AnnotationType WHERE AnnotationTypeId = param_AnnotationTypeId) THEN
        -- Update existing record
        UPDATE AnnotationType
        SET
            TypeName = param_TypeName,
            ValueInScore = param_ValueInScore,
            VisibleToCollaborator = param_VisibleToCollaborator,
            Percentage = param_Percentage
        WHERE
            AnnotationTypeId = param_AnnotationTypeId;
    ELSE
        -- Insert new record
        INSERT INTO AnnotationType (TypeName, ValueInScore, VisibleToCollaborator, Percentage)
        VALUES (param_TypeName, param_ValueInScore, param_VisibleToCollaborator, param_Percentage);
    END IF;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_all_annotations(param_minannotationdate timestamp without time zone, param_maxannotationdate timestamp without time zone)
 RETURNS TABLE(annotationid integer, collaboratorfirstname character varying, collaboratorlastname character varying, collaboratordnicollaborator character varying, collaboratoroperatornumber integer, collaboratoremail character varying, annotationtypename character varying, date date)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY 
    SELECT 
        A.AnnotationId,
        C.FirstName as CollaboratorFirstName,
        C.LastName as CollaboratorLastName,
        C.DNICollaborator as CollaboratorDNICollaborator,
        C.OperatorNumber as CollaboratorOperatorNumber,
        C.Email as CollaboratorEmail,
        AT.TypeName as AnnotationTypeName,
        A.AnnotationDate as Date
    FROM 
        Annotation A
    INNER JOIN 
        Collaborator C ON A.CollaboratorId = C.CollaboratorId
    INNER JOIN 
        AnnotationType AT ON A.AnnotationTypeId = AT.AnnotationTypeId
    LEFT JOIN 
        Annotation_Attachment AA ON A.AttachmentId = AA.AttachmentId
    WHERE
        (A.AnnotationDate >= param_minannotationdate)
        AND (A.AnnotationDate <= param_maxannotationdate);
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_all_announcements_art()
 RETURNS TABLE(announcementartid integer, image character varying, begindatepublication timestamp without time zone, enddatepublication timestamp without time zone, description character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY SELECT * FROM Announcement_Art;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_all_attend()
 RETURNS TABLE(attendanceid integer, collaboratorid integer, checkin timestamp without time zone, checkout timestamp without time zone, checkinstatus integer, checkoutstatus integer, commentcheckin character varying, isopencheckin boolean, ipaddress character varying, physicaladdressequipment character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY 
    SELECT 
        collaborator_attend.attendanceid, 
        collaborator_attend.collaboratorid, 
        collaborator_attend.checkin, 
        collaborator_attend.checkout, 
        collaborator_attend.checkinstatus, 
        collaborator_attend.checkoutstatus, 
        collaborator_attend.commentcheckin, 
        collaborator_attend.isopencheckin, 
        collaborator_attend.ipaddress, 
        collaborator_attend.physicaladdressequipment
    FROM 
        public.collaborator_attend;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_all_bank()
 RETURNS TABLE(bankid integer, name character varying, acronym character varying, accountpattern character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY 
    SELECT 
        b.BankId,
        b.Name,
        b.Acronym,
        b.AccountPattern
    FROM 
        Bank b;
END;
$function$;

CREATE OR REPLACE FUNCTION public.select_all_cantons_by_province(param_provinceid integer)
 RETURNS TABLE(cantonid integer, name character varying, description character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT
        ac.CantonId,
        ac.CantonName,
        ac.Description
    FROM
        Address_Canton ac
    WHERE
        ac.ProvinceId = param_ProvinceId;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_all_collaborators_by_isactive(param_isactive boolean)
 RETURNS TABLE(collaboratorid integer, firstname character varying, lastname character varying, dnicollaborator character varying, operatornumber integer, dateofbirth date, telephone1 character varying, telephone2 character varying, email character varying, parent boolean, maritalstatusid integer, provincename character varying, cantonname character varying, districtname character varying, addressline character varying, gender integer)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT 
        c.CollaboratorId AS collaboratorid,
        c.FirstName AS firstname,
        c.LastName AS lastname,
        c.DNICollaborator AS dnicollaborator,
        c.OperatorNumber AS operatornumber,
        c.DateOfBirth AS dateofbirth,
        c.Telephone1 AS telephone1,
        c.Telephone2 AS telephone2,
        c.Email AS email,
        c.Parent AS parent,
        c.MaritalStatusId AS maritalstatusid,
        ap.ProvinceName AS provinceName,
        ac.CantonName AS cantonName,
        ad.DistrictName AS districtName,
        c.AddressLine AS addressLine,
        c.Gender AS gender
    FROM 
        Collaborator c
        LEFT JOIN Address_Province ap ON c.ProvinceId = ap.ProvinceId
        LEFT JOIN Address_Canton ac ON c.CantonId = ac.CantonId
        LEFT JOIN Address_District ad ON c.DistrictId = ad.DistrictId
    WHERE 
        c.IsActive = param_isactive;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_attend_by_id(param_attendid integer)
 RETURNS TABLE(attendanceid integer, collaboratorid integer, checkin timestamp without time zone, checkout timestamp without time zone, checkinstatus integer, checkoutstatus integer, commentcheckin character varying, isopencheckin boolean, ipaddress character varying, physicaladdressequipment character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY 
    SELECT 
        collaborator_attend.attendanceid, 
        collaborator_attend.collaboratorid, 
        collaborator_attend.checkin, 
        collaborator_attend.checkout, 
        collaborator_attend.checkinstatus, 
        collaborator_attend.checkoutstatus, 
        collaborator_attend.commentcheckin, 
        collaborator_attend.isopencheckin, 
        collaborator_attend.ipaddress, 
        collaborator_attend.physicaladdressequipment
    FROM 
        public.collaborator_attend
    WHERE 
        collaborator_attend.attendanceid = param_attendid;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_attendance_by_date_range(param_startdate timestamp without time zone, param_enddate timestamp without time zone)
 RETURNS TABLE(attendanceid integer, collaboratorid integer, checkin timestamp without time zone, checkout timestamp without time zone, checkinstatus integer, checkoutstatus integer, commentcheckin character varying, isopencheckin boolean, ipaddress character varying, physicaladdressequipment character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF param_startdate = param_enddate THEN
        -- Si las fechas son iguales, traer registros de esa fecha
        RETURN QUERY
        SELECT 
            ca.attendanceid,
            ca.collaboratorid,
            ca.checkin,
            ca.checkout,
            ca.checkinstatus,
            ca.checkoutstatus,
            ca.commentcheckin,
            ca.isopencheckin,
            ca.ipaddress,
            ca.physicaladdressequipment
        FROM public.collaborator_attend ca
        WHERE ca.checkin::date = param_startdate::date -- Solo registros del día especificado
        ORDER BY ca.checkin;
    ELSE
        -- Si las fechas son diferentes, traer el rango de fechas incluyendo los límites
        RETURN QUERY
        SELECT 
            ca.attendanceid,
            ca.collaboratorid,
            ca.checkin,
            ca.checkout,
            ca.checkinstatus,
            ca.checkoutstatus,
            ca.commentcheckin,
            ca.isopencheckin,
            ca.ipaddress,
            ca.physicaladdressequipment
        FROM public.collaborator_attend ca
        WHERE ca.checkin >= param_startdate 
          AND ca.checkin < param_enddate + interval '1 day' -- Incluir registros en la fecha final
        ORDER BY ca.checkin;
    END IF;
END;
$function$;

CREATE OR REPLACE FUNCTION public.select_collaborator_account(param_operatornumber integer)
 RETURNS TABLE(collaboratorid integer, operatornumber integer, password text, needpasswordchange boolean, islockedout boolean, lockoutendtime timestamp without time zone, isactive boolean, passwordchangedate timestamp without time zone)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY SELECT * FROM (
        SELECT
            col.collaboratorid,  
            col.operatornumber,
            Cph.PasswordHash,
            col.NeedPasswordChange,
            col.IsLockedOut,
            col.LockOutEndTime,
            col.IsActive,
		    Cph.PasswordChangeDate
        FROM Collaborator AS col                    
        LEFT JOIN Collaborator_PasswordHistory AS Cph ON col.collaboratorid = Cph.collaboratorid
        ORDER BY Cph.PasswordChangeDate DESC
    ) As col
    WHERE col.operatornumber = param_operatorNumber LIMIT 1;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_collaborator_attendance(param_colaboratorid integer)
 RETURNS TABLE(attendanceid integer, collaboratorid integer, checkin timestamp without time zone, checkout timestamp without time zone, checkinstatus integer, checkoutstatus integer, commentcheckin character varying, isopencheckin boolean, ipaddress character varying, physicaladdressequipment character varying, picture text, firstname character varying, lastname character varying, labelcheckinstatus character varying, labelcheckoutstatus character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY 
    SELECT 
        ca.attendanceid,
        ca.collaboratorid,
        ca.checkin,
        ca.checkout,
        ca.checkinstatus,
        ca.checkoutstatus,
        ca.commentcheckin,
        ca.isopencheckin,
        ca.ipaddress,
        ca.physicaladdressequipment,
        cp.picture,
        c.firstname,
        c.lastname,
        scs.labelcheckinstatus,  -- Corregido
        sos.labelcheckoutstatus  -- Corregido
    FROM 
        public.collaborator_attend ca
    JOIN 
        public.collaborator_picture cp 
        ON ca.collaboratorid = cp.collaboratorid
    JOIN 
        public.collaborator c 
        ON ca.collaboratorid = c.collaboratorid
    JOIN 
        public.schedule_checkinstatus scs 
        ON ca.checkinstatus = scs.checkinstatusid  -- JOIN adicional para checkinstatus
    JOIN 
        public.schedule_checkoutstatus sos 
        ON ca.checkoutstatus = sos.checkoutstatusid  -- JOIN adicional para checkoutstatus
    WHERE 
        ca.collaboratorid = param_colaboratorid;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_collaborator_by_operatornumber(param_operatornumber integer)
 RETURNS TABLE(collaboratorid integer, operatornumber integer, firstname character varying, lastname character varying, email character varying, telephone1 character varying, needpasswordchange boolean, islockedout boolean, lockoutendtime timestamp without time zone, isactive boolean, passwordhash text, passwordchangedate timestamp without time zone, isactual boolean)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY 
    SELECT 
        c.collaboratorid,
        c.operatornumber,
        c.firstname,
        c.lastname,
        c.email,
        c.telephone1,
        c.needpasswordchange,
        c.islockedout,
        c.lockoutendtime,
        c.isactive,
        cph.passwordhash,
        cph.passwordchangedate,
        cph.isactual
    FROM 
        collaborator AS c
    LEFT JOIN 
        collaborator_passwordhistory AS cph 
    ON 
        c.collaboratorid = cph.collaboratorid
    WHERE 
        c.operatornumber = param_operatornumber
    ORDER BY 
        cph.passwordchangedate DESC
    LIMIT 1;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_collaborator_without_active_schedule()
 RETURNS TABLE(collaboratorid integer, firstname character varying, lastname character varying, operatornumber integer)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT
        c.CollaboratorId AS collaboratorid,
        c.FirstName AS firstname,
        c.LastName AS lastname,
        c.OperatorNumber AS operatornumber
    FROM
        Collaborator c
    WHERE NOT EXISTS (
        SELECT 1
        FROM Collaborator_Schedule_History sch
        WHERE c.CollaboratorId = sch.CollaboratorId AND sch.Active = true
    );
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_connection_policy_by_id(param_loginattemptpolicyid integer)
 RETURNS SETOF sessionaccessattemptpolicy
 LANGUAGE plpgsql
AS $function$
BEGIN
	RETURN QUERY SELECT * FROM sessionaccessattemptpolicy WHERE loginattemptpolicyid = param_loginattemptpolicyid;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_collaborator_details_by_id(param_collaboratorid integer)
 RETURNS TABLE(collaboratorid integer, operatornumber integer, firstname character varying, lastname character varying, email character varying, dnicollaborator character varying, dateofbirth date, gender integer, parent boolean, maritalstatusid integer, telephone1 character varying, telephone2 character varying, rfidcode character varying, districtname character varying, cantonname character varying, provincename character varying, addressline character varying, bankname character varying, currencytypename character varying, numberbankaccount character varying, ibanaccount character varying, diseases character varying, takingmedications boolean, note character varying, picture text, emergencycontacts text[])
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY 
    SELECT 
        c.CollaboratorId,
        c.OperatorNumber,
        c.FirstName,
        c.LastName,
        c.Email,
        c.DNICollaborator,
        c.DateOfBirth,
        c.Gender,
        c.Parent,
        c.MaritalStatusId,
        c.Telephone1,
        c.Telephone2,
        c.RFIDCode,
		ad.DistrictName,
		ac.CantonName,
		ap.ProvinceName,
        c.AddressLine,
        b.Name,
        ct.Name,
        cb.NumberBankAccount,
        cb.IBANAccount,
        chc.Diseases,
        chc.TakingMedications,
        chc.Note,
        cp.Picture,
        array_agg(cec.FirstName || ',' || cec.LastName || ',' || cec.Relationship || ',' || cec.Telephone1 || ',' || cec.Telephone2) AS EmergencyContacts
    FROM Collaborator c
	LEFT JOIN Address_Province ap ON c.provinceId = ap.provinceId
	LEFT JOIN Address_Canton ac ON c.cantonId = ac.cantonId
	LEFT JOIN Address_District ad ON c.districtId = ad.districtId
    LEFT JOIN Collaborator_BankAccount cb ON c.CollaboratorId = cb.CollaboratorId
    LEFT JOIN Bank b ON cb.BankId = b.BankId
	LEFT JOIN CurrencyType ct ON ct.CurrencyTypeId = cb.CurrencyTypeId	
    LEFT JOIN Collaborator_HealthCondition chc ON c.CollaboratorId = chc.CollaboratorId
    LEFT JOIN Collaborator_Picture cp ON c.CollaboratorId = cp.CollaboratorId
    LEFT JOIN Collaborator_EmergencyContact cec ON c.CollaboratorId = cec.CollaboratorId
    WHERE c.CollaboratorId = param_collaboratorId
	GROUP BY 
        c.CollaboratorId,
        c.OperatorNumber,
        c.FirstName,
        c.LastName,
        c.Email,
        c.DNICollaborator,
        c.DateOfBirth,
        c.Gender,
        c.Parent,
        c.MaritalStatusId,
        c.Telephone1,
        c.Telephone2,
        c.RFIDCode,
		ad.DistrictName,
		ac.CantonName,
		ap.ProvinceName,
        c.AddressLine,
		b.Name,
		ct.Name,
        cb.NumberBankAccount,
        cb.IBANAccount,
        chc.Diseases,
        chc.TakingMedications,
        chc.Note,
        cp.Picture;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_collaborator_full_by_id(param_collaboratorid integer)
 RETURNS TABLE(collaboratorid integer, operatornumber integer, firstname character varying, lastname character varying, email character varying, dnicollaborator character varying, dateofbirth date, gender integer, parent boolean, maritalstatusid integer, telephone1 character varying, telephone2 character varying, curriculumfile text, rfidcode character varying, needpasswordchange boolean, islockedout boolean, lockoutendtime timestamp without time zone, createuseraccount boolean, isactive boolean, districtid integer, cantonid integer, provinceid integer, addressline character varying, bankid integer, currencytypeid integer, numberbankaccount character varying, ibanaccount character varying, diseases character varying, takingmedications boolean, note character varying, picture text, emergencycontacts text[])
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY 
    SELECT 
        c.CollaboratorId,
        c.OperatorNumber,
        c.FirstName,
        c.LastName,
        c.Email,
        c.DNICollaborator,
        c.DateOfBirth,
        c.Gender,
        c.Parent,
        c.MaritalStatusId,
        c.Telephone1,
        c.Telephone2,
        c.CurriculumFile,
        c.RFIDCode,
        c.NeedPasswordChange,
        c.IsLockedOut,
        c.LockOutEndTime,
		NOT EXISTS(SELECT 1 FROM Collaborator_PasswordHistory cph WHERE param_collaboratorId = cph.collaboratorId),
        c.IsActive,
        c.DistrictId,
        c.CantonId,
        c.ProvinceId,
        c.AddressLine,
		b.BankId,
        ct.CurrencyTypeId,
        cb.NumberBankAccount,
        cb.IBANAccount,
        chc.Diseases,
        chc.TakingMedications,
        chc.Note,
        cp.Picture,
        array_agg(cec.FirstName || ',' || cec.LastName || ',' || cec.Relationship || ',' || cec.Telephone1 || ',' || cec.Telephone2) AS EmergencyContacts
    FROM Collaborator c
	LEFT JOIN Address_Province ap ON c.provinceId = ap.provinceId
	LEFT JOIN Address_Canton ac ON c.cantonId = ac.cantonId
	LEFT JOIN Address_District ad ON c.districtId = ad.districtId
    LEFT JOIN Collaborator_BankAccount cb ON c.CollaboratorId = cb.CollaboratorId
    LEFT JOIN Bank b ON cb.BankId = b.BankId
	LEFT JOIN CurrencyType ct ON ct.CurrencyTypeId = cb.CurrencyTypeId	
    LEFT JOIN Collaborator_HealthCondition chc ON c.CollaboratorId = chc.CollaboratorId
    LEFT JOIN Collaborator_Picture cp ON c.CollaboratorId = cp.CollaboratorId
    LEFT JOIN Collaborator_EmergencyContact cec ON c.CollaboratorId = cec.CollaboratorId
    WHERE c.CollaboratorId = param_collaboratorId
	GROUP BY 
        c.CollaboratorId,
        c.OperatorNumber,
        c.FirstName,
        c.LastName,
        c.Email,
        c.DNICollaborator,
        c.DateOfBirth,
        c.Gender,
        c.Parent,
        c.MaritalStatusId,
        c.Telephone1,
        c.Telephone2,
        c.CurriculumFile,
        c.RFIDCode,
        c.NeedPasswordChange,
        c.IsLockedOut,
        c.LockOutEndTime,
        c.IsActive,
        c.DistrictId,
        c.CantonId,
        c.ProvinceId,
        c.AddressLine,
		b.BankId,
		ct.CurrencyTypeId,
        cb.NumberBankAccount,
        cb.IBANAccount,
        chc.Diseases,
        chc.TakingMedications,
        chc.Note,
        cp.Picture;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_collaborator_last_passwords(param_collaboratorid integer)
 RETURNS TABLE(passwordhash text)
 LANGUAGE plpgsql
AS $function$
BEGIN
	RETURN QUERY 
	SELECT uph.passwordHash FROM Collaborator_PasswordHistory AS UPH
	WHERE collaboratorid = param_collaboratorid
	ORDER BY uph.PasswordChangeDate DESC LIMIT 10;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_collaborator_lastweek_birthday()
 RETURNS TABLE(collaboratorid integer, firstname character varying, lastname character varying, dateofbirth date)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT 
        c.CollaboratorId, 
        c.FirstName, 
        c.LastName, 
        c.DateOfBirth
    FROM 
        Collaborator c
    WHERE 
        (
            to_char(c.DateOfBirth, 'MM-DD') BETWEEN 
            to_char(CURRENT_DATE - INTERVAL '7 day', 'MM-DD') 
            AND 
            to_char(CURRENT_DATE - INTERVAL '1 day', 'MM-DD')
        );
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_collaborator_nextweek_birthday()
 RETURNS TABLE(collaboratorid integer, firstname character varying, lastname character varying, dateofbirth date)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT 
        c.CollaboratorId, 
        c.FirstName, 
        c.LastName, 
        c.DateOfBirth
    FROM 
        Collaborator c
    WHERE 
        (
            to_char(c.DateOfBirth, 'MM-DD') BETWEEN 
            to_char(CURRENT_DATE + INTERVAL '1 day', 'MM-DD') 
            AND 
            to_char(CURRENT_DATE + INTERVAL '7 day', 'MM-DD')
        );
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_collaborator_picture(param_collaboratorid integer)
 RETURNS TABLE(collaboratorid integer, picture text, firstname character varying, lastname character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY 
    SELECT 
        c.collaboratorid,  
        cp.picture,
        c.firstname,
        c.lastname
    FROM 
        public.collaborator c
    LEFT JOIN 
        public.collaborator_picture cp 
        ON c.collaboratorid = cp.collaboratorid
    WHERE 
        c.collaboratorid = param_colLaboratorid;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_collaborator_schedule_info()
 RETURNS TABLE(collaboratorid integer, firstname character varying, lastname character varying, dnicollaborator character varying, operatornumber integer, email character varying, collaboratorschedulehistoryid integer, assigndate timestamp without time zone, schedulename character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT
        c.CollaboratorId,
        c.FirstName,
        c.LastName,
        c.DNICollaborator,
        c.OperatorNumber,
        c.Email,
		sch.CollaboratorScheduleHistoryId,
        sch.AssignDate,
        s.ScheduleName
    FROM Collaborator c
    INNER JOIN Collaborator_Schedule_History sch ON c.CollaboratorId = sch.CollaboratorId
    INNER JOIN Schedule s ON sch.ScheduleId = s.ScheduleId
	WHERE sch.Active = true;

    RETURN;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_collaborator_today_birthday()
 RETURNS TABLE(collaboratorid integer, firstname character varying, lastname character varying, dateofbirth date)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT 
        c.CollaboratorId, 
        c.FirstName, 
        c.LastName, 
        c.DateOfBirth
    FROM 
        Collaborator c
    WHERE 
        to_char(c.DateOfBirth, 'MM-DD') = to_char(CURRENT_DATE, 'MM-DD');
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_collaborator_unique_data()
 RETURNS TABLE(collaboratorid integer, email character varying, telephone1 character varying, operatornumber integer, dnicollaborator character varying)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT
        c.CollaboratorId, 
        c.Email, 
        c.Telephone1, 
        c.OperatorNumber, 
        c.DNICollaborator
    FROM
        Collaborator c;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_schedule_full_by_id(param_scheduleid integer)
 RETURNS TABLE(scheduleid integer, workingdayid integer, name character varying, assignedschedule boolean, scheduledailys text[])
 LANGUAGE plpgsql
AS $function$
DECLARE
    combined_result text[];
    sd_id integer;
    sd_name character varying;
    sd_begintime timestamp without time zone;
    sd_endtime timestamp without time zone;
    s_id integer;
    s_workingdayid integer;
    s_name character varying;
    s_assigned boolean;
BEGIN
    -- Inicializar el arreglo para schedule_daily
    combined_result := ARRAY[]::text[];

    -- Consultar schedule y almacenar el resultado en variables
    SELECT
        s.scheduleid,
        s.workingdayid,
        s.schedulename,
        s.assignedschedule
    INTO
        s_id,
        s_workingdayid,
        s_name,
        s_assigned
    FROM
        public.schedule s
    WHERE
        s.scheduleid = param_scheduleid;

    -- Consultar schedule_daily y agregar resultados al arreglo
    FOR sd_id, sd_name, sd_begintime, sd_endtime IN
        SELECT
            sd.scheduledailyid,
            d.dayname AS scheduledayname,
            sd.begintime,
            sd.endtime
        FROM
            public.schedule_daysbyschedule ds
        INNER JOIN
            public.schedule_daily sd ON ds.scheduledailyid = sd.scheduledailyid
        INNER JOIN
            public.schedule_days d ON sd.dayid = d.dayid
        WHERE
            ds.scheduleid = param_scheduleid
    LOOP
        combined_result := array_append(combined_result, 
            'ScheduleDailyId: ' || sd_id || 
            ', ScheduleDayName: ' || sd_name || 
            ', BeginTime: ' || sd_begintime || 
            ', EndTime: ' || sd_endtime);
    END LOOP;

    -- Retornar los resultados
    RETURN QUERY 
    SELECT s_id, s_workingdayid, s_name, s_assigned, combined_result;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_schedule_history_by_collaborator_id(param_collaboratorid integer)
 RETURNS TABLE(collaboratorschedulehistoryid integer, collaboratorid integer, scheduleid integer, active boolean, assigndate timestamp without time zone, dismissdate timestamp without time zone)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT 
        csh.collaboratorschedulehistoryid,  
        csh.collaboratorid,
        csh.scheduleid,
        csh.active,
        csh.assigndate,
        csh.dismissdate
    FROM 
        public.collaborator_schedule_history AS csh  
    WHERE 
        csh.collaboratorid = param_collaboratorid;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_scheduledaily_by_collaboratorid(param_collaboratorid integer)
 RETURNS TABLE(scheduledailyid integer, dayid integer, dayname character varying, begintime timestamp without time zone, endtime timestamp without time zone)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    WITH active_schedule AS (
        SELECT scheduleid
        FROM public.collaborator_schedule_history
        WHERE collaboratorid = param_collaboratorid
          AND active = TRUE
    ),
    schedule_days AS (
        SELECT sds.scheduledailyid, sds.scheduleid
        FROM public.schedule_daysbyschedule sds
        WHERE sds.scheduleid IN (SELECT scheduleid FROM active_schedule)
    )
    SELECT sd.scheduledailyid, sd.dayid, ds.dayname, sd.begintime, sd.endtime
    FROM public.schedule_daily sd
    JOIN schedule_days sds ON sd.scheduledailyid = sds.scheduledailyid
    JOIN public.schedule_days ds ON sd.dayid = ds.dayid;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_scheduledaily_by_scheduleid(param_scheduleid integer)
 RETURNS TABLE(scheduledailyid integer, dayid integer, begintime timestamp without time zone, endtime timestamp without time zone)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT sd.scheduledailyid, sd.dayid, sd.begintime, sd.endtime
    FROM public.schedule_daily sd
    JOIN public.schedule_daysbyschedule sds
    ON sd.scheduledailyid = sds.scheduledailyid
    WHERE sds.scheduleid = param_scheduleId;
END;
$function$;

CREATE OR REPLACE FUNCTION public.select_user_account_by_id(param_useraccountid integer)
 RETURNS TABLE(useraccountid integer, username character varying, firstname character varying, lastname character varying, email character varying, telephone1 character varying, needpasswordchange boolean, islockedout boolean, lockoutendtime timestamp without time zone, isactive boolean)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT
        ua.UserAccountId,
        ua.UserName,
        ua.FirstName,
        ua.LastName,
        ua.Email,
        ua.Telephone1,
        ua.NeedPasswordChange,
        ua.IsLockedOut,
        ua.LockoutEndTime,
        ua.IsActive
    FROM
        UserAccount ua
    WHERE
        ua.UserAccountId = param_UserAccountId;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_user_account_by_username(param_username character varying)
 RETURNS TABLE(useraccountid integer, username character varying, firstname character varying, lastname character varying, email character varying, telephone1 character varying, needpasswordchange boolean, islockedout boolean, lockoutendtime timestamp without time zone, isactive boolean, passwordhash text, passwordchangeddate timestamp without time zone, isactual boolean)
 LANGUAGE plpgsql
AS $function$
BEGIN
        RETURN QUERY SELECT * FROM (SELECT
        ua.UserAccountId,
        ua.UserName,
		ua.Firstname,
		ua.Lastname,															
        ua.email,
		ua.telephone1,
        ua.NeedPasswordChange,
        ua.IsLockedOut,
	    ua.LockOutEndTime,
        ua.IsActive,
        Uph.PasswordHash,
        Uph.PasswordChangeDate,	
		Uph.IsActual	
        FROM UserAccount AS ua
        LEFT JOIN User_PasswordHistory AS Uph ON ua.UserAccountId = Uph.UserAccountId ORDER BY Uph.PasswordChangeDate DESC) As us
        WHERE us.UserName = param_username or us.email = param_username LIMIT 1;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_user_last_passwords(param_useraccountid integer)
 RETURNS TABLE(passwordhash text)
 LANGUAGE plpgsql
AS $function$
BEGIN
	RETURN QUERY 
	SELECT uph.passwordHash FROM User_PasswordHistory AS uph
	WHERE uph.useraccountid = param_useraccountid
	ORDER BY uph.PasswordChangeDate DESC LIMIT 10;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.select_working_day_all()
 RETURNS TABLE(workingdayid integer, name character varying, description character varying, maxdays integer, maxhours integer, starttime timestamp without time zone, endtime timestamp without time zone, recordtime timestamp without time zone, accumulative boolean)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY 
    SELECT 
        hjl.workingdayid, 
        hjl.workingdayname, 
        hjl.description, 
        hjl.workingdaymaxdays, 
        hjl.workingdaymaxhours,  
        (CURRENT_DATE + hjl.workingdaystarttime)::timestamp without time zone, 
        (CURRENT_DATE + hjl.workingdayendtime)::timestamp without time zone, 
        hjl.workingdayrecordtime, 
        hjl.workingdayaccumulative
    FROM 
        public.schedule_workingday as hjl;         
END;
$function$;

 CREATE OR REPLACE FUNCTION public.update_collaborator_account(param_collaboratorid integer, param_isactive boolean, param_islockedout boolean, param_lockoutendtime timestamp without time zone, param_needpasswordchange boolean, param_operatornumber integer, param_password text, param_passwordchangedate timestamp without time zone)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    UPDATE public.collaborator
    SET 
        isactive = param_isactive,
        islockedout = param_islockedout,
        lockoutendtime = param_lockoutendtime,
        needpasswordchange = param_needpasswordchange,
        operatornumber = param_operatornumber
    WHERE collaboratorid = param_collaboratorid;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.update_collaborator_password(param_collaboratorid integer, param_password character varying, param_needpasswordchange boolean)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    -- Actualizar el historial de contraseñas
    UPDATE collaborator_passwordhistory 
    SET isActual = false 
    WHERE CollaboratorId = param_collaboratorid;
    
    INSERT INTO collaborator_passwordhistory(CollaboratorId, PasswordHash, passwordchangedate, IsActual) 
    VALUES (param_collaboratorid, param_password, now(), true);

    -- Actualizar el campo needpasswordchange en la tabla collaborator
    UPDATE collaborator
    SET needpasswordchange = param_needpasswordchange
    WHERE CollaboratorId = param_collaboratorid;

END;
$function$;

 CREATE OR REPLACE FUNCTION public.update_user_account(param_useraccountid integer, param_username character varying, param_firstname character varying, param_lastname character varying, param_email character varying, param_telephone1 character varying, param_needpasswordchange boolean, param_islockedout boolean, param_lockoutendtime timestamp without time zone, param_isactive boolean)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    UPDATE UserAccount
    SET
        UserName = param_UserName,
        FirstName = param_FirstName,
        LastName = param_LastName,
        Email = param_Email,
        Telephone1 = param_Telephone1,
        NeedPasswordChange = param_NeedPasswordChange,
        IsLockedOut = param_IsLockedOut,
        LockoutEndTime = param_LockoutEndTime,
        IsActive = param_IsActive
    WHERE
        UserAccountId = param_UserAccountId;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.update_user_passwordhistory(param_useraccountid integer, param_passwordhash text, param_needpasswordchange boolean)
 RETURNS void
 LANGUAGE plpgsql
AS $function$
BEGIN
    -- Set IsActual to FALSE for all previous password history entries of the user
    UPDATE User_PasswordHistory
    SET IsActual = FALSE
    WHERE UserAccountId = param_UserAccountId AND IsActual = TRUE;
    
    -- Insert the new password history entry
    INSERT INTO User_PasswordHistory (
        UserAccountId, 
        PasswordHash, 
        PasswordChangeDate, 
        IsActual
    ) VALUES (
        param_UserAccountId, 
        param_PasswordHash, 
        NOW(), 
        TRUE
    );
	
	UPDATE UserAccount
	SET NeedPasswordChange = param_needPasswordChange
	WHERE UserAccountId = param_UserAccountId;
END;
$function$;

  CREATE OR REPLACE FUNCTION public.verify_collaborator_unlock(param_collaboratorid integer, param_maxinvalidattempts integer, param_invalidattemptstime interval, param_loginlockouttime interval)
 RETURNS TABLE(islockedout boolean, lockoutendtime timestamp without time zone)
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF ((SELECT COUNT(*)
        FROM (SELECT *
              FROM collaborator_connectionattempt
              WHERE collaboratorid = param_collaboratorid
              ORDER BY logintime DESC LIMIT param_maxinvalidattempts) AS logins
        WHERE issuccess = FALSE) >= param_maxinvalidattempts) 
      AND (SELECT (SELECT MAX(logintime) - MIN(logintime)
                 FROM (SELECT *
                       FROM collaborator_connectionattempt
                       WHERE issuccess = FALSE AND collaboratorid = param_collaboratorid
                       ORDER BY logintime DESC LIMIT param_maxinvalidattempts) AS logins) < param_invalidattemptstime) 
      AND (SELECT NOT issuccess
           FROM collaborator_connectionattempt
           WHERE collaboratorid = param_collaboratorid
           ORDER BY logintime DESC LIMIT 1)
      AND (SELECT CURRENT_TIMESTAMP AT TIME ZONE 'America/Costa_Rica' - (SELECT MIN(logintime)
                            FROM (SELECT *
                                FROM collaborator_connectionattempt
                                WHERE issuccess = FALSE AND collaboratorid = param_collaboratorid
                                ORDER BY logintime DESC LIMIT param_maxinvalidattempts) AS logins) < param_invalidattemptstime) 
    THEN 
        UPDATE collaborator 
        SET islockedout = TRUE, lockoutendtime = (timezone('America/Costa_Rica', now()) + param_loginlockouttime) 
        WHERE collaboratorid = param_collaboratorid;
    END IF;

    RETURN QUERY
    SELECT c.islockedout, c.lockoutendtime
    FROM collaborator c
    WHERE c.collaboratorid = param_collaboratorid;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.verify_user_unlock(param_useraccountid integer, param_maxinvalidattempts integer, param_invalidattemptstime interval, param_loginlockouttime interval)
 RETURNS TABLE(islockedout boolean, lockoutendtime timestamp without time zone)
 LANGUAGE plpgsql
AS $function$
BEGIN
    IF ((SELECT COUNT(*)
        FROM (SELECT *
              FROM User_ConnectionAttempt
              WHERE useraccountid = param_useraccountid
              ORDER BY logintime DESC LIMIT param_maxinvalidattempts) AS logins
        WHERE issuccess = FALSE) >= param_maxinvalidattempts) 
      AND (SELECT (SELECT MAX(logintime) - MIN(logintime)
                 FROM (SELECT *
                       FROM User_ConnectionAttempt
                       WHERE issuccess = FALSE AND useraccountid = param_useraccountid
                       ORDER BY logintime DESC LIMIT param_maxinvalidattempts) AS logins) < param_invalidattemptstime) 
      AND (SELECT NOT issuccess
           FROM User_ConnectionAttempt
           WHERE useraccountid = param_useraccountid
           ORDER BY logintime DESC LIMIT 1)
      AND (SELECT CURRENT_TIMESTAMP AT TIME ZONE 'America/Costa_Rica' - (SELECT MIN(logintime)
                            FROM (SELECT *
                                FROM User_ConnectionAttempt
                                WHERE issuccess = FALSE AND useraccountid = param_useraccountid
                                ORDER BY logintime DESC LIMIT param_maxinvalidattempts) AS LOGINS) < param_invalidattemptstime) 
    THEN 
        UPDATE UserAccount 
        SET IsLockedOut = TRUE, LockOutEndTime = (timezone('America/Costa_Rica', now()) + param_loginlockouttime) 
        WHERE useraccountid = param_useraccountid;
    END IF;

    RETURN QUERY
    SELECT ua.IsLockedOut, ua.LockOutEndTime
    FROM UserAccount ua
    WHERE ua.useraccountid = param_useraccountid;
END;
$function$;

 CREATE OR REPLACE FUNCTION public.select_workingday_by_collaboratorid(param_collaboratorid integer)
 RETURNS TABLE(workingdayid integer, name character varying, description character varying, maxdays integer, maxhours integer, starttime timestamp without time zone, endtime timestamp without time zone, recordtime timestamp without time zone, accumulative boolean, assigned boolean)
 LANGUAGE plpgsql
AS $function$
BEGIN
    RETURN QUERY
    SELECT 
        wd.workingdayid AS "WorkingDayId",
        wd.workingdayname AS "Name",
        wd.description AS "Description",
        wd.workingdaymaxdays AS "MaxDays",
        wd.workingdaymaxhours AS "MaxHours",
        -- Convertir starttime y endtime a timestamp
        (CURRENT_DATE + wd.workingdaystarttime)::timestamp AS "StartTime",
        (CURRENT_DATE + wd.workingdayendtime)::timestamp AS "EndTime",
        wd.workingdayrecordtime AS "RecordTime",
        wd.workingdayaccumulative AS "Accumulative",
        wd.workingdayassigned AS "Assigned"
    FROM 
        public.collaborator_schedule_history csh
    JOIN 
        public.schedule s ON csh.scheduleid = s.scheduleid
    JOIN 
        public.schedule_workingday wd ON s.workingdayid = wd.workingdayid
    WHERE 
        csh.collaboratorid = param_collaboratorid
        AND csh.active = TRUE  -- Considerar solo registros activos
    ORDER BY 
        csh.assigndate DESC  -- Ordenar por la fecha de asignación, si es necesario
    LIMIT 1;  -- Limitar a un solo resultado si solo deseas la jornada más reciente
END;
$function$;

CREATE OR REPLACE FUNCTION public.calculate_schedule_difference(
	param_collaboratorid integer,
	param_dayid integer,
	param_minutesworkedday integer)
    RETURNS TABLE(extratime text, pendingtime text) 
    LANGUAGE 'plpgsql'
    COST 100
    VOLATILE PARALLEL UNSAFE
    ROWS 1000

AS $BODY$
DECLARE
    scheduled_time_minutes INTEGER;
    time_difference INTEGER;
BEGIN
    -- Seleccionar el horario según CollaboratorId y DayId
    WITH active_schedule AS (
        SELECT scheduleid
        FROM public.collaborator_schedule_history
        WHERE collaboratorid = param_collaboratorid
          AND active = TRUE
    ),
    schedule_days AS (
        SELECT sds.scheduledailyid, sd.dayid, sd.begintime, sd.endtime
        FROM public.schedule_daily sd
        JOIN public.schedule_daysbyschedule sds ON sd.scheduledailyid = sds.scheduledailyid
        WHERE sds.scheduleid IN (SELECT scheduleid FROM active_schedule)
    )
    SELECT EXTRACT(EPOCH FROM (endtime - begintime)) / 60 INTO scheduled_time_minutes
    FROM schedule_days
    WHERE dayid = param_dayid;

    -- Validar si se encontró un horario para el día
    IF scheduled_time_minutes IS NULL THEN
        RAISE EXCEPTION 'No se encontró horario para el CollaboratorId % y DayId %', param_collaboratorid, param_dayid;
    END IF;

    -- Calcular la diferencia de tiempo
    time_difference := param_minutesworkedday  - scheduled_time_minutes;

    -- Retornar los tiempos extra o pendientes en el formato deseado
    IF time_difference >= 0 THEN
        RETURN QUERY 
        SELECT 
            CONCAT((time_difference / 60), 'h ', (time_difference % 60), 'm') AS extraTime,
            '0h 0m' AS pendingTime;
    ELSE
        RETURN QUERY 
        SELECT 
            '0h 0m' AS extraTime,
            CONCAT(ABS(time_difference) / 60, 'h ', ABS(time_difference) % 60, 'm') AS pendingTime;
    END IF;
END;
$BODY$;