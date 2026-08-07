export interface CourseSubjectModel {

  id: string;

  courseId: string;

  courseName: string;

  subjectId: string;

  subjectName: string;

  isMandatory: boolean;

  displayOrder: number;

}

export interface CreateCourseSubjectModel {

  courseId: string;

  subjectId: string;

  isMandatory: boolean;

  displayOrder: number;

}

export interface UpdateCourseSubjectModel {

  courseId: string;

  subjectId: string;

  isMandatory: boolean;

  displayOrder: number;

}