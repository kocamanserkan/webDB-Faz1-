const form = document.querySelector('form');
const input = document.querySelector('#txtColumnName')
const btnDeleteAll = document.querySelector('#btnDeleteAll')
const columnList = document.querySelector('#column-list')
const table = document.getElementById('tr');
let columnArray =[];
const btnCreateTable = document.querySelector('#btnCreateTable');
const btnPreview = document.querySelector('#btnPreview');
const x = document.querySelector('#x');
const btnCancel = document.querySelector('#btnCancel');
const inputCard = document.querySelector('#inputTable');
const btnAddNewColumn = document.querySelector('#btnAddNewColumn');
const previewTableName = document.querySelector('#previewTableName');
const tableName = document.querySelector('#txtTableName');

eventListeners();


function eventListeners(){

    //submit column
    form.addEventListener('submit',addNewColumn);
   
    //updateColumn
    columnList.addEventListener('click',updateColumn);
    //delete a column
    columnList.addEventListener('click',deleteColumn);
     //delete al columns
    btnDeleteAll.addEventListener('click',deleteAllColumns);
    //Show Preview
    btnPreview.addEventListener('click',showPreview)
    //Cancel
    btnCancel.addEventListener('click',cancel)
    //Create Table
    btnCreateTable.addEventListener('click',createTable);
   

   
}

function createItem(text){
    //create li
    const li = document.createElement('li');
    const span = document.createElement('span');
    // span.className ='ui-icon ui-icon-arrowthick-2-n-s'
    // li.className='list-group-item list-group-item-secondary';
    li.className='ui-state-default';
    
    li.appendChild(document.createTextNode(text));
    //create a
    const a = document.createElement('a');
    const b = document.createElement('a');
    
    a.classList = 'delete-item float-right';
    a.setAttribute('href','#');
    a.innerHTML ='<i class="fas fa-times" title="Sil" tooltip="Sil" style="font-size:24px; "></i>';
    b.classList = 'delete-item float-right';
    b.setAttribute('href','#');
    b.innerHTML ='<i class="fas fa-edit" title="Düzenle" style="font-size:24px; margin-right: 15px;"></i>';
    //add a to li
    // li.appendChild(span);
    li.appendChild(a);
    li.appendChild(b);
 
    //add li to a
    columnList.appendChild(li);

}


  
    if(btnAddNewColumn.hasAttribute('style'))
    {
        
        console.log(columnArray,'update öncesi')
        let updateIndex = localStorage.getItem('update')
        columnArray.splice(updateIndex,1,input.value);
        console.log(columnList.children[updateIndex],'güncellenecek olan nesne')
        console.log(columnArray,'update sonrası')
        console.log(typeof(updateIndex),'indexxxxx')
        /*   columnList.children[updateIndex].remove();*/
        columnList.children[updateIndex].textContent = 'a';
       /* createItem(input.value);*/
        
        
        console.log(columnList,'kolon listesi')


        localStorage.removeItem('update');
        btnAddNewColumn.removeAttribute('style');
        input.value='';
    }
    else
    {
       
        if(input.value == '')
        {
            alert('Lütfen kolon adı giriniz.');
        }
        else
        {
            if(columnControl(input.value))
            {
    
            //create item
    
            createItem(input.value);
            
            columnArray.push(String(input.value).toLocaleLowerCase('en-US'));
            //clear input
            input.value='';
    
            }
            else
            {
                alert(`${input.value} adlı kolon eklemiş durumda.`);
                input.value='';
            }
        }
    }

        
        e.preventDefault();
    }
    
  
   
function deleteColumn(e){
 
    if(e.target.className === 'fas fa-times' && confirm('Kolonu silmek istediğinden emin misin?')){
        index = columnArray.indexOf((e.target.parentElement.parentElement.textContent.toLocaleLowerCase('en-US')))
   
        e.target.parentElement.parentElement.remove();
       
      
        columnArray.splice(index,1);
        
        console.log(columnArray,'DeleteColumn')
      
     }
    

     e.preventDefault();
}
function updateColumn(e) { 

    
    if(e.target.className === 'fas fa-edit'){
      
    index = columnArray.indexOf((e.target.parentElement.parentElement.textContent.toLocaleLowerCase('en-US')))
    input.value = e.target.parentElement.parentElement.textContent;
    btnAddNewColumn.setAttribute('style','background:darkred')

    localStorage.setItem('update',index);


     }

 }

function deleteAllColumns(e){
    if(columnArray.length>0){
        if(confirm('Bütün kolonları silmek istediğinden emin misin?')){
            columnList.innerHTML='';
            table.innerHTML ='';
            columnArray = [];
            
            console.log(columnArray,'deleteall')
        }
        
    }
    else{
        alert('Kolon girilmemiştir.')
    }
   

    e.preventDefault();
}

function previewTable(title){

    // var html = `
    // <th>${title}</th>`;

    // table.innerHTML += html;
    
}

function createTable(){
    columnArray.push(tableName.value);
    if(columnArray.length<1){
        alert('Lütfen Tabloya kolon ekleyin');

    }
    else{
        alert('Tablo Oluşturuldu.');

    }
    console.log(columnArray)




}
function columnControl(columnName){

    if(columnArray.includes(String(columnName).toLocaleLowerCase('en-US'))){
        
        return false;
    }
    else{
     
        return true;
    }
}


//$(function () {
//    $( "#column-list" ).sortable({
       
//    });
//    $( "#column-list" ).disableSelection();
//  } );


  function showPreview(e) {

    if(columnArray.length>0){
        table.innerHTML = '';
        previewTableName.textContent= `Tablo Adı: ${tableName.value}`;

        for(let i=0;i<columnArray.length;i++){
            var html = `
            <th>${columnArray[i]}</th>`;
    
            table.innerHTML += html;
        }
    
        $("#previewCard").toggle();
        x.setAttribute('style','opacity:0.5; pointer-events:none;' )
        inputCard.setAttribute('style','opacity:0.5; pointer-events:none;' )
        
    }
    else{
        alert('Ön izleme için tabloya kolon eklemelisiniz..')
    }

   
 
    
  }
  function cancel(e) { 
    x.setAttribute('style','opacity:1; pointer-events:all;' )
    inputCard.setAttribute('style','opacity:1; pointer-events:all;' )
    
    $("#previewCard").hide();
   
   }