1.	IProcedureItemData
•	Inherits from IDatabaseObject<ProcedureItem>
•	Methods:
	•	IEnumerable<ProcedureItem> Parent(string id)
2.	IParentChildData
•	Methods:
	•	IEnumerable<ParentChild> ReadAll()
	•	ParentChild Read(string id)
	•	bool Write(ParentChild parentChild)
	•	bool Update(ParentChild parentChild)
	•	bool Delete(string id)
3.	IProcedureData
•	Inherits from IDatabaseObject<Procedure>
•	Methods:
	•	bool AddProcedureItem(ProcedureItem procedureItem)
	•	bool RemoveProcedureItem(string procedureItemId)
Classes
1.	ParentChildData
•	Implements IParentChildData
•	Dependencies:
	•	IDataService _dataService
•	Methods:
	•	IEnumerable<ParentChild> ReadAll()
	•	ParentChild Read(string id)
	•	bool Write(ParentChild parentChild)
	•	bool Update(ParentChild parentChild)
	•	bool Delete(string id)
2.	ProcedureData
•	Implements IProcedureData
•	Dependencies:
	•	IDataService _dataService
	•	IProcedureItemData _procedureItemData
•	Methods:
	•	IEnumerable<Procedure> ReadAll()
	•	Procedure Read(string id)
	•	bool Write(Procedure procedure)
	•	bool Update(Procedure procedure)
	•	bool Delete(string id)
	•	bool AddProcedureItem(ProcedureItem procedureItem)
	•	bool RemoveProcedureItem(string procedureItemId)
Relationships
•	ParentChildData implements IParentChildData.
•	ProcedureData implements IProcedureData.
•	ProcedureData depends on IProcedureItemData and IDataService.
•	ParentChildData depends on IDataService.
