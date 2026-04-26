using Girvs.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HaoKao.LearnProgressService.Infrastructure.Migrations
{
    public partial class addStoredProcedure : GirvsMigration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_CourseLearningProgressStatistics`;
              CREATE DEFINER=`root`@`%` PROCEDURE `Sp_CourseLearningProgressStatistics`(IN studentId CHAR(36),IN productId CHAR(36), IN tenantId CHAR(36),IN subjectId CHAR(36),OUT couseCount INT,OUT couseIsEndCount INT)
BEGIN
		DECLARE baseSql VARCHAR(5000);
		CALL Sp_CreateBaseSql(studentId,productId,tenantId,subjectId,baseSql);
		set @querySql=baseSql;
		-- SELECT @querySql;
		
	  set @querycouseCountSql=CONCAT(@querySql,'\nSELECT count(1) into @couseCount from temp6;');
		PREPARE stmt FROM  @querycouseCountSql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
		set couseCount=@couseCount;
	  -- SELECT @couseCount;
		
		set @queryCouseIsEndCountSql=CONCAT(@querySql,'\nSELECT count(1) into @couseIsEndCount from temp6 WHERE IsEnd=1;');
		PREPARE stmt FROM  @queryCouseIsEndCountSql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
		set couseIsEndCount=@couseIsEndCount;
		
		set @queryCouseCategorySql=CONCAT(@querySql,'\n,temp7 as (SELECT  SubjectId,SubjectName,Category,Sum(Duration) as ''Duration'',IFNULL(Sum(MaxProgress),0) as ''MaxProgress'' from temp6 GROUP BY SubjectId,SubjectName,Category )');
		set @queryCouseCategorySql=CONCAT(@queryCouseCategorySql,'\nSELECT * from temp7;');
		PREPARE stmt FROM  @queryCouseCategorySql; EXECUTE stmt; DEALLOCATE PREPARE stmt;

END");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_CreateBaseSql`;
              CREATE DEFINER=`root`@`%` PROCEDURE `Sp_CreateBaseSql`(IN studentId CHAR(36),IN productId CHAR(36), IN tenantId CHAR(36),IN subjectId CHAR(36),OUT baseSql VARCHAR(5000))
BEGIN
		 set @tenantIdSuffix=CONCAT('_',REPLACE(tenantId,'-',''));
		 -- SELECT @tenantIdSuffix;
    -- 查找产品服务库名
		 SELECT Table_Schema into @Product_Schema FROM information_schema.`TABLES` WHERE Table_Name='ProductPackage' ORDER BY Table_Rows desc LIMIT 1;
		 -- SELECT @Product_Schema;
		 
			-- 查找课程服务库名
		 SELECT Table_Schema into @Course_Schema FROM information_schema.`TABLES` WHERE Table_Name like '%CourseChapter%' ORDER BY Table_Rows desc LIMIT 1;
		 -- SELECT @Course_Schema;
		 
			-- 查找学习进度服务库名
		 SELECT Table_Schema into @LearnProgress_Schema FROM information_schema.`TABLES` WHERE Table_Name like '%LearnProgress%' ORDER BY Table_Rows desc LIMIT 1;
		 -- SELECT @LearnProgress_Schema;
		 
		 set @querySql=CONCAT('WITH temp2 as (SELECT  p.SubjectId,p.SubjectName,p.PermissionName,p.PermissionId,p.Category FROM  ',@Product_Schema,'.ProductPermission p  WHERE p.SubjectId=''',subjectId,''' AND ProductId=''',productId,''')');
		   
		 set @querySql=CONCAT(@querySql,'\n,temp3 as (SELECT temp2.*,couseChapter.Id as ''CourseChapterId'',couseChapter.Name as ''CourseChapterName'' FROM temp2 LEFT join ',@Course_Schema,'.CourseChapter',@tenantIdSuffix,' couseChapter on temp2.PermissionId=couseChapter.CourseId )');
		 set @querySql=CONCAT(@querySql,'\n,temp4 as (SELECT temp3.*,courseVideo.Id as ''CourseVideoId'',FLOOR(courseVideo.Duration/60) as ''Duration'',courseVideo.VideoName from temp3 LEFT join ',@Course_Schema,'.CourseVideo',@tenantIdSuffix,' courseVideo on temp3.CourseChapterId=courseVideo.CourseChapterId  WHERE courseVideo.Duration is not NULL)');
		 set @querySql=CONCAT(@querySql,'\n,temp5 as (SELECT temp4.*,FLOOR(learnProgress.Progress/60) as ''Progress'',FLOOR(learnProgress.TotalProgress/60) as ''TotalProgress'',FLOOR(learnProgress.MaxProgress/60) as ''MaxProgress'',learnProgress.CreateTime,learnProgress.IsEnd from temp4 LEFT JOIN ',@LearnProgress_Schema,'.LearnProgress',@tenantIdSuffix,' learnProgress on temp4.CourseVideoId=learnProgress.VideoId AND learnProgress.CreatorId=''',StudentId,''')');
		 set @querySql=CONCAT(@querySql,'\n,temp6 as (SELECT SubjectId,SubjectName,PermissionName,PermissionId,CourseChapterId,CourseChapterName,CourseVideoId,Category,Duration,VideoName,Max(TotalProgress) as ''TotalProgress'',Max(MaxProgress) as ''MaxProgress'',Max(CreateTime) as ''CreateTime'',Max(IsEnd) as ''IsEnd'' FROM temp5 GROUP BY    SubjectId,SubjectName,PermissionName,PermissionId,CourseChapterId,CourseChapterName,CourseVideoId,Category,Duration,VideoName )');
		 set baseSql=@querySql;
		  --  SELECT baseSql;
END");
            migrationBuilder.Sql(@"DROP PROCEDURE IF EXISTS `Sp_QueryCourseLearningProgress`;
            CREATE DEFINER=`root`@`%` PROCEDURE `Sp_QueryCourseLearningProgress`( IN studentId CHAR(36),IN productId CHAR(36), IN tenantId CHAR(36),IN subjectId CHAR(36),IN pageIndex INT,IN pageSize INT,out totalRows INT)
BEGIN
		DECLARE baseSql VARCHAR(5000);
		CALL Sp_CreateBaseSql(studentId, productId,tenantId,subjectId,baseSql);
		set @querySql=baseSql;
	  set @querySql=CONCAT(@querySql,',\ntemp7 as (SELECT SubjectId,SubjectName,PermissionName,PermissionId,CourseChapterId,CourseChapterName,Sum(Duration) as ''Duration'' , IFNULL(Sum(TotalProgress),0)  as ''TotalProgress'' ,IFNULL(Sum(MaxProgress),0) as ''MaxProgress'' , IFNULL( Max(CreateTime),''0001-01-01 00:00:00'') as ''CreateTime'' FROM temp6 GROUP BY SubjectId,SubjectName,PermissionName,PermissionId,CourseChapterId,CourseChapterName ) ');
		  -- SELECT @querySql;
			
		 set @querySqlCount=CONCAT(@querySql,'\nSELECT Count(1) into @totalRows from temp7;');
		 PREPARE stmt FROM @querySqlCount; EXECUTE stmt; DEALLOCATE PREPARE stmt;
		 set totalRows=@totalRows;
		 -- SELECT count;
		 
		 set @querySqlPage=CONCAT(@querySql,'\nSELECT * from temp7 ORDER BY PermissionId ,CourseChapterId Limit ',pageSize,' OFFSET ', (pageIndex-1)*pageSize,';');
		 -- SELECT @querySqlPage;
     PREPARE stmt FROM @querySqlPage; EXECUTE stmt; DEALLOCATE PREPARE stmt;
	
END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
