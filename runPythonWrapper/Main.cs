using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using ExecutionEngine;
using System.Linq;

namespace Test
{
	class MainClass
	{	
		public static void Main (string[] args)
		{						
			//var testProgram = TestProgram.GetTestPrograms().Where(f => f.Name.Contains("Java") && f.Name.Contains("Hello")).Single();					
			//var testProgram = TestProgram.GetTestPrograms().Where(f => f.Name.Contains("Python") && f.Name.Contains("Hello")).Single();	
			//var testProgram = TestProgram.GetTestPrograms().Where(f => f.Name.Contains("C_") && f.Name.Contains("Hello")).Single();	
			//var testProgram = TestProgram.GetTestPrograms().Where(f => f.Name.Contains("Javascript") && f.Name.Contains("Hello")).Single();	
				
			
			var testProgram = TestProgram.GetTestPrograms().Where(f => f.Name.Contains("CClang") && f.Name.Contains("_Hello")).Single();	
			//TestEngineThroughService(testProgram.Program, testProgram.Input, testProgram.Lang);
			TestEngineDirectly(testProgram.Program, testProgram.Input, testProgram.Lang, testProgram.Args);
			
			
			//var res = Diff.GetDiff("", "");
			//Console.WriteLine(res.IsError);
			//Console.WriteLine(res.Result);
		}
		
		static void TestEngineThroughService(string Program, string Input, Languages Lang)
		{
			OutputData odata;
			bool bytes = true;
			//using(var s = new localhost.Service())
			//using(var s = new localhost_2.Service())
			using(var s = new n226589_s_dedikuoti_lt.Service())
			{
				
				Stopwatch watch = new Stopwatch();
				watch.Start();
				//Test.localhost.Result res = s.DoWork(Program, Input, (Test.localhost.Languages)Lang);	
				Test.n226589_s_dedikuoti_lt.Result res = s.DoWork(Program, /*Input,*/ (Test.n226589_s_dedikuoti_lt.Languages)Lang, GlobalUtils.TopSecret.ServiceUser, GlobalUtils.TopSecret.ServicePass, bytes);	
				
				//Test.localhost_2.Result res = s.DoWork(Program, (Test.localhost_2.Languages)Lang, "", "", bytes);
				
				watch.Stop();
				if(res != null)
				{
					if(string.IsNullOrEmpty(res.Stats))
						res.Stats = "";
					else
						res.Stats += ", ";
					res.Stats += string.Format("absolute service time: {0} sec", Math.Round((double)watch.ElapsedMilliseconds/(double)1000, 2));
				}
				
				odata = new OutputData()
					{
						Errors = res.Errors,
						Warnings = res.Warnings,
						Stats = res.Stats,
						Output = res.Output,
						Exit_Status = res.Exit_Status,
						System_Error = res.System_Error
					};
				if(bytes)
				{
					if(res.Errors_Bytes != null)
						odata.Errors = System.Text.Encoding.Unicode.GetString(res.Errors_Bytes);
					if(res.Warnings_Bytes != null)
						odata.Warnings = System.Text.Encoding.Unicode.GetString(res.Warnings_Bytes);
					if(res.Output_Bytes != null)
						odata.Output = System.Text.Encoding.Unicode.GetString(res.Output_Bytes);
				}
			}
			ShowData(odata);
			
		}
		static void TestEngineDirectly(string Program, string Input, Languages Lang, string Args = null)
		{
			Engine engine = new Engine();
			InputData idata = new InputData()
			{
				Program = Program,
				Input = Input,
				Lang = Lang,
				Compiler_args = Args
			};
			var odata = engine.DoWork(idata);		
			ShowData(odata);			
		}
		
		static void ShowData(OutputData odata)
		{
			if(!string.IsNullOrEmpty(odata.System_Error))
			{
				Console.WriteLine("System error:");
				Console.WriteLine(odata.System_Error);				
			}
			else
			{
				Console.WriteLine("Errors:");
				Console.WriteLine(odata.Errors);
				Console.WriteLine("Warnings:");
				Console.WriteLine(odata.Warnings);
				Console.WriteLine("Output:");	
				Console.WriteLine(odata.Output);		
				Console.WriteLine("Exit status:");	
				Console.WriteLine(odata.Exit_Status);	
				Console.WriteLine("Stats:");	
				Console.WriteLine(odata.Stats);	
			}
			Console.ReadLine();
		}
		
		class TestProgram
		{
			public string Name
			{
				get;
				set;
			}
			public string Program
			{
				get;
				set;
			}
			public string Input
			{
				get;
				set;				
			}
			public Languages Lang
			{
				get;
				set;
			}
			public string ShouldContain
			{
				get;
				set;
			}
			public string Args
			{
				get;
				set;
			}
			public static List<TestProgram> GetTestPrograms()
			{
				List<TestProgram> list = new List<TestProgram>();
			
				#region javascript
				list.Add(new TestProgram()
				         {
							Name = "Javascript_Hello",
							Input = @"27
412
42
",
							Lang = Languages.Javascript,
							Program = @"while((numdsgdsfgdfg = readline()) != 42) {
	print(num);
}"
						 });
				
				#endregion
				
				#region JAVA
				
				
				list.Add(new TestProgram()
				         {
							Name = "Java_1",
							Program = @"
class Rextester
{
   public static void main(String args[]) {
       System.out.println(new Foo());
       (new java.util.Date()).getDate();
   }

}

class Foo {

   public String toString() {
       String.format(""Foo!"");
   }

}",
							Input = "5",
							Lang = Languages.Java
						 });

				list.Add(new TestProgram()
				         {
							Name = "Java_Bug",
							Program = @"
class Rextester
{
   public static void main(String args[]) {
       System.out.println(new Foo());
   }

}

class Foo {

   public String toString() {
       String.format(""Foo!"");
   }

}",
							Input = "5",
							Lang = Languages.Java
						 });
				list.Add(new TestProgram()
				         {
							Name = "Java_Hello",
							Program = @"
//Title of this code
//'main' method must be in a class 'Rextester'.

import java.util.*;
import java.lang.*;
import java.nio.charset.*;

class Rextester
{  
    public static void main(String args[])
    {
        Scanner in = new Scanner(System.in);
        System.out.println(in.nextInt());
    }
    
}",
							Input = "5",
							Lang = Languages.Java
						 });
				

				list.Add(new TestProgram()
				         {
							Name = "Java_Loop_",
							Program = @"
import java.util.*;
import java.lang.*;

class Rextester
{
	public static void main (String[] args) throws java.lang.Exception
	{		
		while(1==1)
			;
	}
}",							
						});
									list.Add(new TestProgram()
				         {
							Name = "Java_LoopPrint",
							Program = @"
import java.util.*;
import java.lang.*;

class Rextester
{
	public static void main (String[] args) throws java.lang.Exception
	{		
		while(1==1)
			System.out.println(""Hello from Java "");
	}
}",
							Lang = Languages.Java
						 });
				list.Add(new TestProgram()
				         {
							Name = "Java_Memory",
							Program = @"
import java.util.*;
import java.lang.*;

class Rextester
{
	public static void main (String[] args) throws java.lang.Exception
	{
		ArrayList<String> list = new ArrayList<String>();
		while(true)
		{
			list.add(""text"");
		}
	}
}",
							Lang = Languages.Java
						 });
list.Add(new TestProgram()
				         {
							Name = "Java_Divide",
							Program = @"
import java.util.*;
import java.lang.*;

class Rextester
{
	public static void main (String[] args) throws java.lang.Exception
	{
		int a = 0;
		System.out.println(5/a);
	}
}",
					Lang = Languages.Java
						 });
list.Add(new TestProgram()
				         {
							Name = "Java_Threads",
							Program = @"
class Rextester extends Thread {

    public void run() {
		(new Rextester()).start();
    }

    public static void main(String args[]) {
        (new Rextester()).start();
    }

}",
					
							Lang = Languages.Java
						 });
				list.Add(new TestProgram()
				         {
							Name = "Java_Warning",
							Program = @"
class Rextester{
	public static void main(String args[]) {
    	java.util.Date d = new java.util.Date();
		system.out.println(d.getDate());
    }
}",
					
							Lang = Languages.Java
						 });
				
				list.Add(new TestProgram()
				         {
							Name = "Java_Time",
							Program = @"
import java.util.Date;
import java.text.DateFormat;
import java.text.SimpleDateFormat;
import java.util.Calendar;

class Rextester
{
    public static void main(String args[]) 
    {
    	DateFormat dateFormat = new SimpleDateFormat(""yyyy/MM/dd HH:mm:ss S"");
	    Date date = new Date();
	    System.out.println(dateFormat.format(date));
    }
}
",
					
							Lang = Languages.Java
						 });
				list.Add(new TestProgram()
				         {
							Name = "Java_Bad",
							Program = @"

class Rextester
{
    public static void main(String args[]) 
    {
}
",
					
							Lang = Languages.Java
						 });	
				list.Add(new TestProgram()
				         {
							Name = "Java_ControlAscii",
							Program = @"

class Rextester
{
    public static void main(String args[]) 
    {
		System.out.println(""���������"");
	}
}
",
					
							Lang = Languages.Java
						 });
			
				
				#endregion
				#region Python

				list.Add(new TestProgram()
				       {
							Program=@"
import sys
print sys.stdin.read()
",
							Lang = Languages.Python,
							Name = "Python_Hello",
							Input = "ęįš",
						});
					list.Add(new TestProgram()
				       {
							Program=@"

a = 1000000
print a
while a>0:
	a=a-1
h = input()
",
							Lang = Languages.Python,
							Name = "Python_Blocked",
						});
				
				
				list.Add(new TestProgram()
				       {
							Program=@"
while 1==1:
	a = 5",
						Lang = Languages.Python,
							Name = "Python_Loop_"
						});
				
								
				list.Add(new TestProgram()
				       {
							Program=@"
while 1==1:
	print ""Hello world""",
						Lang = Languages.Python,
							Name = "Python_LoopPrint"
						});
				
				list.Add(new TestProgram()
				       {
							Program=@"
a = 0
b =5/a",
							Lang = Languages.Python,
							Name = "Python_Divide"
						});
				
				
												list.Add(new TestProgram()
				       {
							Program=@"
a = []
while 1==1:
	a.append('text')
",
							Lang = Languages.Python,
							Name = "Python_Memory"
						});
				
				list.Add(new TestProgram()
				       {
							Program=@"
import resource
resource.setrlimit(resource.RLIMIT_CPU, (10, 10))
",
							Lang = Languages.Python,
							Name = "Python_Limit"
						});
				list.Add(new TestProgram()
				       {
							Program=@"
				
#import getpass
#print getpass.getuser()
from subprocess import call
call([""rm"", ""/home/ren/Desktop/example.txt""])
",
							Lang = Languages.Python,
							Name = "Python_RemoveFile"
						});
					list.Add(new TestProgram()
				       {
							Program=@"
				
f = open('/tmp/sometest', 'w')
print f
f.write('')
",
							Lang = Languages.Python,
							Name = "Python_CreateFile"
						});
					list.Add(new TestProgram()
				       {
							Program=@"
from subprocess import call
call([""mkdir"", ""/tmp/wow""])
",
							Lang = Languages.Python,
							Name = "Python_MakeDirectory"
						});
				
			list.Add(new TestProgram()
				       {
							Program=@"
import random
#while 1==1:
f = open('/tmp/sometest'+str(random.randint(1,1000000)), 'w')
f.write('')
f.close()
",
							Lang = Languages.Python,
							Name = "Python_WriteFiles"
						});
					list.Add(new TestProgram()
				       {
							Program=@"
from subprocess import call
call([""ln"", ""-s"", ""/home/ren/Desktop/ho.txt"", ""/tmp/test1000""])
",
							Lang = Languages.Python,
							Name = "Python_Link"
						});
				list.Add(new TestProgram()
				       {
							Program=@"
from subprocess import call
call([""chmod"", ""a+w"", ""/home/ren/Desktop/example.txt""])
",
							Lang = Languages.Python,
							Name = "Python_Chmod"
						});
				
								list.Add(new TestProgram()
				       {
							Program=@"
while 1==1:
	from subprocess import call
	call([""java"", ""FileWrite""])
",
							Lang = Languages.Python,
							Name = "Python_CallJavaInLoop"
						});
				
							list.Add(new TestProgram()
				       {
							Program=@"
import warnings
warnings.warn('This is a warning message')
print 5
",
							Lang = Languages.Python,
							Name = "Python_Warning"
						});
				
				
					list.Add(new TestProgram()
				       {
							Program=@"
from subprocess import call
call([""chmod"", ""000"", ""/""])
",
							Lang = Languages.Python,
							Name = "Python_Evil1"
						});
				
					list.Add(new TestProgram()
				       {
							Program=@"
from subprocess import call
call([""sudo"", ""passwd"", ""root""])
",
							Lang = Languages.Python,
							Name = "Python_Evil2"
						});

					list.Add(new TestProgram()
				       {
							Program=@"
from subprocess import call
call([""rm"", ""-rf"", "".*""])
",
							Lang = Languages.Python,
							Name = "Python_Evil3"
						});
					list.Add(new TestProgram()
				       {
							Program=@"
import os
f=os.popen(""bomb(){ bomb|bomb& };bomb"")
",
							Lang = Languages.Python,
							Name = "Python_Evil4"
						});

					
					list.Add(new TestProgram()
				       {
							Program=@"
import os
#os.setpgrp()
while 1==1:
    os.system(""xsp &"")
",
							Lang = Languages.Python,
							Name = "Python_Evil5"
						});
				
				
					list.Add(new TestProgram()
				       {
							Program=@"
import time
from subprocess import call
call([""xsp""])
time.sleep(10)
",
							Lang = Languages.Python,
							Name = "Python_Daemon"
						});
				list.Add(new TestProgram()
				       {
							Program=@"
import datetime

print 'python self - ' + str(datetime.datetime.now())
",
							Lang = Languages.Python,
							Name = "Python_Time"
						});
				#endregion
				#region C and C++
				list.Add(new TestProgram()
				       {
							Program=@"
#include  <stdio.h>
#include  <sys/types.h>

void  main(void)
{

     pid_t  pid;

     pid = fork();
     if (pid == 0) 
	 {
		while(1==1)
			fork();
	 }	
     else 
     {
		while(1==1)
			fork();
 	 }
}
",
							Lang = Languages.C,
							Name = "C_Fork"
						});
								list.Add(new TestProgram()
				       {
							Program=@"
#include  <stdio.h>
#include  <sys/types.h>

int  main(void)
{

     for(int i=0; i<8; i++)
		printf(""Hi!"");
}
",
							Lang = Languages.C,
							Name = "C_Hello"
						});
						list.Add(new TestProgram()
				       {
							Program=@"
#include <sys/resource.h>

int which = PRIO_PROCESS;
int pid;
int priority = -20;
int ret;

void  main(void)
{
	pid = getpid();
	ret = setpriority(which, pid, priority);
	printf(""pid %d ret %d\n"", pid, ret);
	
}
",
							Lang = Languages.C,
							Name = "C_Nice"
						});	
				
				list.Add(new TestProgram()
				       {
							Program=@"
#include  <stdio.h>
int main()
{
    float f ;
    
    printf (""grade ?"");
    scanf (""%f"", & f);


    
        
if( f < 4.0 );
   printf (""You had a %d.\nYou deserve $0"", f);   
  
elif(f < 5.0);
     printf (""You had a %d.\nYou deserve $10"", f);
elif(f < 7.0 );
         printf (""You had a %d.\nYou deserve $100"", f);
elif( f == 7.0 );
            printf (""You had a %d.\n You deserve $500"", f);
                   
    return 0;

}
",
							Lang = Languages.C,
							Name = "C_NoErrorWords"
						});	

				
				list.Add(new TestProgram()
				       {
							Program=@"
#include <iostream>
using namespace std;

enum class Enumeration {
    Val1,
    Val2,
    Val3 = 100,
    Val4 // = 101
};

int main ()
{


	for(int i=0; i<5; i++)
		cout << ""Hi"";
}",
							Lang = Languages.CPP,
							Name = "CPP_Hello"
						});
				
				
				list.Add(new TestProgram()
				       {
							Program=@"
#include  <stdio.h>

int main(void)
{
	for(int i=0; i<5; i++)
		cout << ""Hi"";
}",
						
							Lang = Languages.C,
							Name = "C_Divide"
						});
				
				list.Add(new TestProgram()
				       {
							Program=@"
#include  <stdio.h>

int main(void)
{
     while(1==1)
		;
}",
							Lang = Languages.C,
							Name = "C_Loop"
						});
				
				list.Add(new TestProgram()
				       {
							Program=@"
#include  <stdio.h>

int main(void)
{
     while(1==1)
		printf(""text\n"");
}",
							Lang = Languages.C,
							Name = "C_LoopPrint"
						});
				
				list.Add(new TestProgram()
				       {
							Program=@"
#include <iostream>
#include <fstream>
using namespace std;

int main () {
  ofstream myfile;
  myfile.open (""/home/ren/Desktop/example.txt"");
if (myfile.is_open())
{
  myfile << ""Writing this to a file.\n"";
  myfile.close();
}
else
{
	cout << ""Couldn't open file."";
}
  return 0;
}
",
							Lang = Languages.CPP,
							Name = "CPP_WriteFile"
						});
				
				
				list.Add(new TestProgram()
				         {
							Name = "C_Memory",
							Program = @"
#include  <stdio.h>

int main(void)
{
	int a = 100000000;	
	int *ptr = (int *) malloc(a * sizeof (int));
	if (ptr == NULL) 
	{
	   printf(""Memory could not be allocated!\n"");
	} 
	else
	{
		printf(""Allocated successfuly!\n"");
		int counter = 0;
		*ptr = counter;
		printf(""%d: %d: %d sizeof int: %d\n\n"",&ptr, ptr, *ptr, sizeof (int));
		
		while(--a>0)
		{	
			ptr++;    
			*ptr=++counter;
			printf(""%d %d\n"", ptr, *ptr);
		}
	}
}
",
							Lang = Languages.C
						 });
				
				list.Add(new TestProgram()
				       {
							Program=@"
#include  <stdio.h>

int main(void)
{
	int a=0;
	int b = 5
}",
							Lang = Languages.C,
							Name = "C_Warning"
						});
				
								list.Add(new TestProgram()
				       {
							Program=@"
#include <iostream>
using namespace std;

int main ()
{
  cout << ""Hello from C++!\n"";
	int a = 0;
	int b = 5;
  return 0;
}",
							Lang = Languages.CPP,
							Name = "CPP_Warning"
						});
					list.Add(new TestProgram()
				       {
							Program=@"
/* C demo code */

#include <zmq.h>
#include <pthread.h>
#include <semaphore.h>
#include <time.h>
#include <stdio.h>
#include <fcntl.h>
#include <malloc.h>

typedef struct {
  void* arg_socket;
  zmq_msg_t* arg_msg;
  char* arg_string;
  unsigned long arg_len;
  int arg_int, arg_command;

  int signal_fd;
  int pad;
  void* context;
  sem_t sem;
} acl_zmq_context;

#define p(X) (context->arg_##X)

void* zmq_thread(void* context_pointer) {
  acl_zmq_context* context = (acl_zmq_context*)context_pointer;
  char ok = 'K', err = 'X';
  int res;

  while (1) {
    while ((res = sem_wait(&context->sem)) == EINTR);
    if (res) {write(context->signal_fd, &err, 1); goto cleanup;}
    switch(p(command)) {
    case 0: goto cleanup;
    case 1: p(socket) = zmq_socket(context->context, p(int)); break;
    case 2: p(int) = zmq_close(p(socket)); break;
    case 3: p(int) = zmq_bind(p(socket), p(string)); break;
    case 4: p(int) = zmq_connect(p(socket), p(string)); break;
    case 5: p(int) = zmq_getsockopt(p(socket), p(int), (void*)p(string), &p(len)); break;
    case 6: p(int) = zmq_setsockopt(p(socket), p(int), (void*)p(string), p(len)); break;
    case 7: p(int) = zmq_send(p(socket), p(msg), p(int)); break;
    case 8: p(int) = zmq_recv(p(socket), p(msg), p(int)); break;
    case 9: p(int) = zmq_poll(p(socket), p(int), p(len)); break;
    }
    p(command) = errno;
    write(context->signal_fd, &ok, 1);
  }
 cleanup:
  close(context->signal_fd);
  free(context_pointer);
  return 0;
}

void* zmq_thread_init(void* zmq_context, int signal_fd) {
  acl_zmq_context* context = malloc(sizeof(acl_zmq_context));
  pthread_t thread;

  context->context = zmq_context;
  context->signal_fd = signal_fd;
  sem_init(&context->sem, 1, 0);
  pthread_create(&thread, 0, &zmq_thread, context);
  pthread_detach(thread);
  return context;
}
",
							Lang = Languages.CPP,
							Name = "CPP_Wierd"
						});
				
				#endregion
				#region php
			list.Add(new TestProgram()
				       {
							Program=@"
<?php
function hello($who) {
	return ""Hello "" . $who;
}
?>
<p>The program says <?= hello(""World"") ?>.</p>
<script>
	alert(""And here is some JS code""); // also colored
</script>

",
							Lang = Languages.Php,
							Name = "Php_Hello"
						});
				
				#endregion
				
				#region pascal
							list.Add(new TestProgram()
				       {
							Program=@"
program hello;

begin
	{$WARNING this is dubious code}
	{while 1=1 do
	begin		
	end;}
	writeln('ėšįęčė„!');
end.
",
							Lang = Languages.Pascal,
							Name = "Pascal_Hello"
						});
				#endregion
				#region Objective C
							list.Add(new TestProgram()
				       {
							Program=@"
  #import <Foundation/Foundation.h>

int main (void)
 {
  NSLog (@""Executing"");
return 0;
}

",
							Lang = Languages.ObjectiveC,
							Name = "Objective_Hello"
						});
				#endregion
				#region
				list.Add(new TestProgram()
				       {
							Program=@"
main = do
       putStrLn ""Greetings!  What is your name?""
       inpStr <- getLine
       putStrLn $ ""Welcome to Haskell, "" ++ inpStr ++ ""!""
						
",
							Lang = Languages.Haskell,
							Name = "Haskell_Hello",
							Input = "abc" 
						});
				#endregion
				#region
				list.Add(new TestProgram()
				       {
							Program=@"
# Output ""I love Ruby""
say = ""I love Ruby""
puts say
 
# Output ""I *LOVE* RUBY""
say['love'] = ""*love*""
puts say.upcase


# Output ""I *love* Ruby""
# five times
5.times { puts say }
",
							Lang = Languages.Ruby,
							Name = "Ruby_Hello"
						});
				#endregion
								#region
				list.Add(new TestProgram()
				       {
							Program=@"
$t = 5/0;
print ""Hello World\n"";
",
							Lang = Languages.Ruby,
							Name = "Perl_Hello"
						});
				#endregion
				#region
				list.Add(new TestProgram()
				       {
							Program=@"
local read, write = io.read, io.write
local num, nl = '*n', '\n'
while true do
  local a = read(num)
  if a == 42 then return end
  write(a, nl)
end
",
							Lang = Languages.Lua,
							Name = "Lua_Hello",
							Input = "41\n42"
						});
				#endregion
				#region
				list.Add(new TestProgram()
				       {
							Program=@"
section .data
	hello:     db 'Hello world!',10    ; 'Hello world!' plus a linefeed character
	helloLen:  equ $-hello             ; Length of the 'Hello world!' string
	                                   ; (I'll explain soon)

section .text
	global _start

_start:
	mov eax,4            ; The system call for write (sys_write)
	mov ebx,1            ; File descriptor 1 - standard output
	mov ecx,hello        ; Put the offset of hello in ecx
	mov edx,helloLen     ; helloLen is a constant, so we don't need to say
	                     ;  mov edx,[helloLen] to get it's actual value
	int 80h              ; Call the kernel

	mov eax,1            ; The system call for exit (sys_exit)
	mov ebx,0            ; Exit with return code of 0 (no error)
	int 80h
",
							Lang = Languages.Nasm,
							Name = "Nasm_Hello"
						});
				#endregion

				#region lisp
				list.Add(new TestProgram()
				         {
					Program=@"
; hello world lisp program.
(print ""Hello World"")
",
					Lang = Languages.Lisp,
					Name = "Lisp_Hello"
				});
				
#endregion

				#region prolog
				list.Add(new TestProgram()
				         {
					Program=@"
likes(apple).",
					Input = "likes(X).",
					Lang = Languages.Prolog,
					Name = "Prolog_Hello"
				});
				
#endregion
				#region prolog
				list.Add(new TestProgram()
				         {
					Program=@"
package main;  import ""fmt""; func main() { fmt.Printf(""hello, world\n"") }dfdsf",
					Lang = Languages.Go,
					Name = "Go_Hello"
				});
				
#endregion
				#region scala
				list.Add(new TestProgram()
				         {
					Program=@"
class Rextester {
  val (x, y): (Int, Int) = (1, 2)
}
",
					Lang = Languages.Scala,
					Name = "Scala_Hello"
				});
				
#endregion

				#region scheme
				list.Add(new TestProgram()
				         {
					Program=@"
(display ""ėųęūė"")
(newline)",
					Lang = Languages.Scheme,
					Name = "Scheme_Hello"
				});
				
#endregion
				#region scheme
				list.Add(new TestProgram()
				         {
					Program=@"
setTimeout(function(){console.log(""Hello World"");},3000);
console.log(""hi"");
",
					Lang = Languages.Nodejs,
					Name = "Node_Hello"
				});
				
#endregion
				#region scheme
				list.Add(new TestProgram()
				         {
					Program=@"
print(""ėūųęįė"")
",
					Lang = Languages.Python3,
					Name = "Python3_Hello"
				});
				
#endregion
								#region scheme
				list.Add(new TestProgram()
				         {
					Program=@"
x = [1,2,3,4,5,6,7,8,9,10];
y = [2,5,7];
plot(x, sin(x));
print -dgif graph.gif;
a = [7]
",
					Lang = Languages.Octave,
					Name = "Octave_Hello"
				});


				
#endregion

				list.Add(new TestProgram()
				       {
							Program=@"
#include <iostream>
using namespace std;

enum class Enumeration {
    Val1,
    Val2,
    Val3 = 100,
    Val4 // = 101
};

int main ()
{


	for(int i=0; i<5; i++)
		cout << ""Hi"";
}",
							Lang = Languages.CppClang,
							Name = "CPPCLANG_Hello",
							Args = "-Wall -std=c++11 -O2 -o a.out source_file.cpp"
						});
				list.Add(new TestProgram()
				       {
							Program=@"
#include  <stdio.h>
#include  <sys/types.h>

int  main(void)
{

     for(int i=0; i<8; i++)
		printf(""Hi!"");
}
",
							Lang = Languages.C,
							Name = "CClang_Hello",
							Args = "-Wall -std=gnu99 -O2 -o a.out source_file.c"
						});

				return list;
			}
		}
		

		
	}
}

