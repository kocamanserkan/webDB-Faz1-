
const form = document.querySelector('#inputForm');
const input = document.querySelector('#txtColumnName')
const columnList = document.querySelector('#column-list')
const btnAddColumn = document.querySelector('#x');
const btnDeleteAll = document.querySelector('#btnDeleteAll');
const btnShowPreview = document.querySelector('#btnShowPreview');
const btnCreateTable = document.querySelector('#btnCreateTable');
const table = document.querySelector('#tr');
const tableName = document.querySelector('#txtTableName');
const previewTableName = document.querySelector('#previewTableName');
const hidden = document.getElementById('<%=hiddenColumListToCB.ClientID%>')
const dataType = document.querySelector('#ddlColumnType');
const cbknullable = document.querySelector('#ckbNullable');






let columnArray = [];
let arrayOld = [];


eventListeners();


function eventListeners() {
    btnAddColumn.ad
    entListener('click', addNewColumn);//submit column
    columnList.addEventListener('click', updateColumn);  ////updateColumn
    columnList.addEventListener('click', deleteColumn);////delete a column
    btnDeleteAll.addEventListener('click', deleteAllColumns); ////delete al columns
    btnShowPreview.addEventListener('click', showPreview) ////Show Preview
    //btnCancel.addEventListener('click', cancel)
    btnCreateTable.addEventListener('click', createTable);//Create Table
}

function createItem(text) {

   /* let litext = text + " / " + dataType*/

    const li = document.createElement('li'); //create li
    li.className = 'ui-state-default';
    li.appendChild(document.createTextNode(text));
  
 
    

    ////create a
    const btnSingleDel = document.createElement('a'); //delete a
    const btnpdateColumn = document.createElement('a'); //update a

    btnSingleDel.classList = 'delete-item float-right';
    btnSingleDel.setAttribute('href', '#');
    btnSingleDel.setAttribute('style', 'btn-danger')
    btnSingleDel.innerHTML = '<i title="Sil" class="icon-trash"></i>';

    btnpdateColumn.classList = 'update-item float-right';
    btnpdateColumn.setAttribute('href', '#');
    btnpdateColumn.innerHTML = '<i title="Güncelle" class="icon-refresh" style="font-size:60px; margin-right: 35px;"></i>';


    //adding a's to Li
    li.appendChild(btnSingleDel);
    li.appendChild(btnpdateColumn);
    columnList.appendChild(li); //adding li to a
}


function addNewColumn(e) {
        
    if (btnAddColumn.hasAttribute('style'))//is Button Update or Add New ?
    {
        if (columnControl(input.value))//Update Operation
        {
            console.log(columnArray, 'Update öncesi liste durumu')
            let updateIndex = localStorage.getItem('update')
            console.log(updateIndex)


            columnArray[updateIndex].name = input.value;
            columnArray[updateIndex].dataType = dataType.value;
            columnArray[updateIndex].nullable = cbknullable.checked;

            console.log(columnArray[updateIndex],'güncellenen nesne')

            //columnArray[updateIndex].name.splice(updateIndex, 1, input.value); //replacing items btwn old and newd
            //columnArray[updateIndex].dataType.splice(updateIndex, 1, dataType.value); //replacing items btwn old and newd
            //columnArray[updateIndex].nullable.splice(updateIndex, 1, cbknullable.checked); //replacing items btwn old and newd

            let newText;
            if (cbknullable.checked) {
                newText = input.value + " / " + dataType.value + " / " + "Boş Geçilebilir"
            } else
            {
                newText = input.value + " / " + dataType.value + " / " + "Boş Geçilemez"
            }



            console.log(columnList.children[updateIndex], 'güncellenecek olan nesne')
            console.log(columnList.children[updateIndex].textContent, 'güncellenecek liste texti')
            console.log(columnArray, 'update sonrası liste durumu')
            var text = '' + newText + '<a class="delete-item float-right" href = "#" style = "btn-danger" > <i title="Sil" class="icon-trash"></i></a > <a class="update-item float-right" href="#"><i title="Güncelle" class="icon-refresh" style="font-size:60px; margin-right: 35px;"></i></a>';
            columnList.childNodes[parseInt(updateIndex) + 1].innerHTML = text
    

            //Update completed. Btn is changed to old version
            btnAddColumn.removeAttribute('style');

            //localStorage den eski itemin silinmesi
            localStorage.removeItem('update');
            btnAddColumn.textContent = 'Ekle';

            input.value = '';

        }
        else
        {
            alert('Güncellenecek olan kolon adı mevcut')
        }

    }
    else {

        if (input.value == '') {
            alert('Lütfen kolon adı giriniz.');
            input.focus();
        }
        else {
            if (columnControl(input.value))
            {

                let = columnObject = {
                    name: input.value,
                    dataType: dataType.value,
                    nullable: cbknullable.checked
                }
                var nul;

                if (cbknullable.checked === true) {
                    var nul = "Boş Geçilebilir";

                }
                else {
                    nul = "Boş geçilemez";
                }


                let createItemText = input.value + " / " + dataType.value + " / " + nul


                //create item
                createItem(createItemText);
                columnArray.push(columnObject);
              /*  columnArray.push(String(input.value).toLocaleLowerCase('en-US'));*/
                console.log(columnArray, 'yeni kolon eklendi. ')
                

                //clear input
                input.value = '';

                
                console.log(columnObject);
               



            }
            else
            {
                alert(`${input.value} adlı kolon eklemiş durumda.`);
                input.value = '';
            }
        }
    }

    e.preventDefault()

}
function updateColumn(e) {


    if (e.target.className === 'icon-refresh') {

        var big = e.target.parentElement.parentElement.textContent.toLocaleLowerCase('en-US')
        var res = big.split(" / ");
        console.log(res[0])

        index = columnArray.indexOf((e.target.parentElement.parentElement.textContent.toLocaleLowerCase('en-US')))
        index = columnArray.findIndex(x => x.name === res[0]);

        console.log(index, "asdasdasdasdasdasdas");

        //input.value = e.target.parentElement.parentElement.textContent;
        input.value = columnArray[index].name
        dataType.value = columnArray[index].dataType
        cbknullable.checked = columnArray[index].nullable
        arrayOld.push(input.value)
        console.log(arrayOld,'eski item (Güncellenecek Olan)')

        localStorage.setItem('update', index);
        btnAddColumn.setAttribute('style', 'background:darkred')


        btnAddColumn.setAttribute('content', 'test content');
        btnAddColumn.textContent = 'Güncelle';
        input.focus();
    }
}


function columnControl(columnName) {

    if (columnArray.includes(String(columnName).toLocaleLowerCase('en-US')))
    {
        return false;
    }
    else
    {
        return true;
    }
}

function deleteColumn(e) {

    if (e.target.className === 'icon-trash' && confirm('Kolonu silmek istediğinden emin misin?')) {
        if (arrayOld.includes(e.target.parentElement.parentElement.textContent.toLocaleLowerCase('en-US')))
        {
            index = columnArray.indexOf((e.target.parentElement.parentElement.textContent.toLocaleLowerCase('en-US')))
            console.log(e.target.parentElement.parentElement.textContent.toLocaleLowerCase('en-US'))
            e.target.parentElement.parentElement.remove();
            columnArray.splice(parseInt(index) + 1, 1);
            console.log(columnArray, 'DeleteColumn','deleted updated item')
        }
        else
        {
            index = columnArray.indexOf((e.target.parentElement.parentElement.textContent.toLocaleLowerCase('en-US')))
            console.log(e.target.parentElement.parentElement.textContent.toLocaleLowerCase('en-US'))
            e.target.parentElement.parentElement.remove();
            columnArray.splice(index, 1);
            console.log(columnArray, 'Deleted never updated item')
        }
    }

    e.preventDefault();
}
function deleteAllColumns(e) {

    if (columnArray.length > 0)
    {
        if (confirm('Bütün kolonları silmek istediğinden emin misin?'))
        {
            columnList.innerHTML = '';
            columnArray = [];
            console.log(columnArray, 'deleteall')
        }

    }
    else
    {
        alert('Kolon girilmemiştir.')
    }


}

function showPreview(e) {

    if (columnArray.length > 0)
    {
        if (tableName.value == '') {
            alert('Tablo adını giriniz')
            tableName.focus();
        }
        else {
            table.innerHTML = '';
            previewTableName.textContent = `Tablo Adı: ${tableName.value}`;
            jQuery.noConflict();
            $('#myModal').modal('show');

            for (let i = 0; i < columnArray.length; i++) {
                var html = `
            <th>${columnArray[i].name}</th>`;
                table.innerHTML += html;
            }

        }
       
        
    }
    else
    {
        alert('Ön izleme için tabloya kolon eklemelisiniz..')
        input.focus();
    }
    e.preventDefault()
}


function createTable(e) {

    columnArray.push(tableName.value);
    if (columnArray.length < 1) {
        alert('Lütfen Tabloya kolon ekleyin');
    }
    else
    {
       
       /* alert('Tablo Oluşturuldu.');*/
        
        sendData(columnArray);
    }
    console.log(columnArray, 'Created table items that will send to codebehind');
    e.preventDefault();
}

function sendData(array1) {
    let a;
    a = '';
    for (var i = 0; i < array1.length; i++) {
        a += array1[i] + '/';
        
    }

    var recipient = document.getElementById('ContentPlaceHolder1_sql');
    recipient.value = a;




    console.log(a);
   
}